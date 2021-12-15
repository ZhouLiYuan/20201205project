using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Role;

/// <summary>
/// 负责UI的管理，加载和生成
/// </summary>
public static class UIManager
{
    //根据角色不同的交互状态显示不同UI 比如 任务-惊叹号 普通状态-向下箭头
    public static string HintUIName => "Arrow";
    public static string LockUIName => "LockUI";

    public static string CanvasName => "Canvas";

    public static Transform CanvasTransform { get; private set; }
    public static Camera UICamera { get; private set; }

    private static Stack<BasePanel> panelStack = new Stack<BasePanel>();
    private static BasePanel currentPanel;
    //new的实例只会存在在内存中，不会在场景中
    private static List<BasePanel> m_panels = new List<BasePanel>();
    private static List<GameObject> m_guiders = new List<GameObject>();

    private static Dictionary<string, GameObject> uiGobjNameDic = new Dictionary<string, GameObject>();


    static UIManager()
    {
        // prefab创建UI环境Gobj
        var handle = Addressables.LoadAssetAsync<GameObject>("UIEnvironment");//可考虑换成同步版本API
        var uiPrefab = handle.WaitForCompletion();
        var ui_Obj = Object.Instantiate(uiPrefab);

        //建立表现层和逻辑层之间关系
        CanvasTransform = ui_Obj.transform.Find("Canvas");
        UICamera = ui_Obj.transform.Find("UICamera").GetComponent<Camera>();
        UICamera.depth = 99;
    }

    #region Panel相关工具(暂时默认同类面板唯一)

    /// <summary>
    ///  创建并获取一个面板实例(同步加载方法)
    ///  建立面板表现层和逻辑层之间关系
    /// </summary>
    /// <typeparam name="TPanel"></typeparam>
    /// <returns></returns>
    public static TPanel OpenPanel<TPanel>(System.Action<TPanel> onPanelOpened = null) where TPanel : BasePanel, new() 
    {
        //panel脚本实例
        var panel = new TPanel();
        var name = typeof(TPanel).Name;
        Debug.Log($"类型名称{name}");

        ConfigPanel(panel, name);
        return panel;
    }

    //OpenPanel<TPanel>的方法重载（内部写法略有不同，效果一样）
    public static TPanel OpenPanel<TPanel>() where TPanel : BasePanel, new()
    {
        TPanel panel = new TPanel();
        //获得name（UI Prefab名必须和 脚本 类名 一致）
        //type.Name仅仅只是把Type类型转换为了string类型
        //ToString也可以实现相同效果，不过type.Name更精准（因为有时不明确ToString的重载）
        System.Type type = typeof(TPanel);
        var name = type.ToString();
        Debug.Log($"类型名称{name}");

        ConfigPanel(panel,name);
        return panel;
    }

    /// <summary>
    /// 通过名称打开Panel(级别BasePanel是基类变量，依然可以调用到子类最新覆盖的Init()方法)
    /// 但注意返回值引用类型只会是BasePanel，无法用其子类变量接受
    /// </summary>
    /// <param name="PanelTypeName"></param>
    /// <returns></returns>
    public static BasePanel OpenPanel(string PanelTypeName)
    { 
        BasePanel panel = new BasePanel();
        ConfigPanel(panel, PanelTypeName);
        return panel;
    }

    private static void ConfigPanel(BasePanel panel,string panelName)
    {
        var prefab = ResourcesLoader.LoadPanelPrefab(panelName);
        var obj = Object.Instantiate(prefab, CanvasTransform);
        obj.name = panelName;
        Object.DontDestroyOnLoad(obj);

        panel.Init(obj);
        panel.OnOpen();

        //设置父类为Canvas
        panel.Transform.SetParent(CanvasTransform, false);
        //往界面 面板集合 中追加元素
        m_panels.Add(panel);
    }


    public static BasePanel GetPanel(string PanelName)
    {
        if (PanelName.Contains("Panel"))
        {
            var targetPanel = m_panels.Find(item => item.GameObject.name.Contains(PanelName));
            return targetPanel;
        }
        else
        {
            Debug.LogError($"场景中不存在{PanelName}");
            return null;
        }
    }

    /// <summary>
    /// 面板专用关闭方法
    /// </summary>
    /// <typeparam name="TPanel"></typeparam>
    /// <param name="panel"></param>
    public static void ClosePanel<TPanel>(TPanel panel) where TPanel : BasePanel, new()
    {
        panel.OnClose();
        //UnityEngine和.Net的System里分别各有一个Object类，需要命名空间+成员访问符UnityEngine.来访问Object
        UnityEngine.Object.Destroy(panel.GameObject);
        m_panels.Remove(panel);
    }

    public static void ClosePanel(string PanelName)
    {
        foreach (var item in m_panels)
        {
            if (item.GameObject.name.Contains(PanelName))
            {
                item.OnClose();
                UnityEngine.Object.Destroy(item.GameObject);
                m_panels.Remove(item);
            }
        }
    }

    //-----------------------------<Panel栈相关>---------------------------------
    /// <summary>
    /// Panel的入栈操作，此操作会显示一个面板
    /// </summary>
    /// <param name="nextPanel"></param>
    public static void PushPanel(BasePanel nextPanel)
    {
        if (panelStack.Count > 0)
        {
            currentPanel = panelStack.Peek();
            currentPanel.OnPause();
            panelStack.Push(nextPanel);
        }
    }

    public static void PopPanel()
    {
        //一个pop弹出后不确定是否有下一个显示所以需要判断两次
        if (panelStack.Count > 0) panelStack.Peek().OnClose();
        if (panelStack.Count > 0) panelStack.Peek().OnResume();
    }

    public static void PopAllPanel()
    {
        //Pop()的返回值是BasePanel
        while (panelStack.Count > 0) { panelStack.Pop().OnClose(); }
    }

    #endregion

    #region GuiderUI工具
    /// <summary>
    /// 在对象的略微上方生成
    /// </summary>
    /// <param name="name"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    public static GameObject SpawnGuiderUI(string name, Transform parent)
    {
        var offset = 1f;
        var guider = ResourcesLoader.LoadGuiderPrefab(name);
        var Gobj = Object.Instantiate(guider, parent, false);
        Gobj.transform.position = new Vector3(Gobj.transform.position.x, Gobj.transform.position.y + offset, Gobj.transform.position.z);
        m_guiders.Add(Gobj);
        return Gobj;
    }

    public static void DestoryGuiderUI(GameObject obj)
    {
        m_guiders.Remove(obj);
        Object.Destroy(obj);
    }
    #endregion

    /// <summary>
    /// 为非boss敌人或者NPC生成血槽
    /// </summary>
    /// <param name="name"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    public static HealthBar SpawnHealthBar(RoleEntity owner)
    {
       
        var HealthBar = ResourcesLoader.LoadHealthBarPrefab();
        var Gobj = Object.Instantiate(HealthBar,CanvasTransform/*.Find("GamePanel")*/);
        //Gobj.name = owner.GetType().Name + "HealthBar";
        Gobj.name = owner.UniqueName + "'s HealthBar";
 

        var healthBar = new HealthBar(Gobj);
        healthBar.SetOwner(owner);
        AdvGamePanel.HealthBarNameDic[owner.UniqueName] = healthBar;
        return healthBar;
        //希望UI类方法可以写的更通用
    }

    #region 生命周期相关
    /// <summary>
    /// 刷新面板信息
    /// </summary>
    public static void OnUpdate()
    {
        foreach (var panel in m_panels) panel.OnUpdate(Time.deltaTime);
    }
    #endregion

    #region 换算工具
    /// <summary>
    /// 换算普通Transform为Rect Transform
    /// </summary>
    /// <param name="target"></param>
    /// <param name="UIGobj"></param>
    public static void SetInteractUIPosition(GameObject target, GameObject UIGobj)
    {
        UIGobj.SetActive(target != null);
        if (target)
        {
            //Postion赋值过程：target =>MainCamera =>UICamera =>lockUI
            var screenPosition = Camera.main.WorldToScreenPoint(target.transform.position);
            UIGobj.transform.position = UICamera.ScreenToWorldPoint(screenPosition);
        }
        //    var m_height = 0f;
        //    //Canvas设置为 Screen Space - Camera 的方法
        //    Vector3 viewPos = Camera.main.WorldToScreenPoint(m_owner.position + new Vector3(0f, m_height, 0f));

        //    //根据比率自动缩放 横轴纵轴？意思是相对长宽？
        //var m_scaler = GameObject.FindObjectOfType<CanvasScaler>();
        //var widthRatio = m_scaler.referenceResolution.x / Camera.main.scaledPixelWidth;
        //var heightRatio = m_scaler.referenceResolution.y / Camera.main.scaledPixelHeight;
        //UIGobj.GetComponent<RectTransform>().anchoredPosition = new Vector2(viewPos.x * widthRatio, viewPos.y * heightRatio);
    }
    /// <summary>
    /// 有offset的重载(写不了可选参数形式就写成重载)
    /// </summary>
    /// <param name="target"></param>
    /// <param name="UIGobj"></param>
    /// <param name=""></param>
    public static void SetInteractUIPosition(GameObject target, GameObject UIGobj, Vector3 offset)
    {
        UIGobj.SetActive(target != null);
        if (target)
        {
            var spawnPos = target.transform.position + offset;
            //Postion赋值过程：target =>MainCamera =>UICamera =>lockUI
            var screenPosition = Camera.main.WorldToScreenPoint(spawnPos);
            UIGobj.transform.position = UICamera.ScreenToWorldPoint(screenPosition);
        }
    }

    #endregion


    #region 未启用的通用方法系列
    public static GameObject GetSingleUI(string UIName)
    {
        //字典只要有key 哪怕没有指定value 也会有个映射null 
        //所以才必须用ContainsKey来判断 映射（value）是否非空
        if (uiGobjNameDic.ContainsKey(UIName)) { return uiGobjNameDic[UIName]; }
        else 
        {
            //Debug.LogError($"场景中不存在{UIName}");
            //return null;

            //如果没有value，就创建一个，再返回
            var uiGobj = AddSingleUI(UIName, CanvasTransform);
            return uiGobj;
        }
    }

    public static GameObject AddSingleUI(string UIName, Transform parent)
    {
        //如果没有value，就创建一个，再返回
        GameObject uiGobj = GameObject.Instantiate(Resources.Load<GameObject>(UIName), parent);//加载方式需要调整
        uiGobj.name = UIName;
        //记得生成的Gobj value也给字典加上
        uiGobjNameDic.Add(UIName, uiGobj);
        return uiGobj;
    }


    /// <summary>
    /// 若不存在 该类型组件 就添加一个 再获取
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T GetOrAddComponent<T>(GameObject targetGobj) where T : Component
    {
        if (targetGobj.GetComponent<T>() == null) { targetGobj.AddComponent<T>(); }

        return targetGobj.GetComponent<T>();
    }

    /// <summary>
    /// 销毁 一个UI对象
    /// </summary>
    /// <param name="info"></param>
    public static void DestroyUI(string UIName)
    {
        GameObject.Destroy(uiGobjNameDic[UIName]);
        uiGobjNameDic.Remove(UIName);
    }

    #endregion




    public static Color GetRandomColor()
    {
        var color = Random.ColorHSV();
        //float r = Random.Range(0f, 1f);
        //float g = Random.Range(0f, 1f);
        //float b = Random.Range(0f, 1f);
        //var color = new Color(r, g, b, 1f);
        return color;
    }

    //-----------------------------<过时方法>---------------------------------
    #region 协程版本
    /// <summary>
    ///  协程方法初始化UI环境canvas（有了同步版本后暂时不用）
    /// </summary>
    /// <returns></returns>
    public static IEnumerator Init()
    {
        // 使用prefab创建已经配置好的UI环境
        var handle = Addressables.LoadAssetAsync<GameObject>("UIEnvironment");

        yield return handle;
        var uiPrefab = handle.Result;

        var ui_Obj = Object.Instantiate(uiPrefab);

        CanvasTransform = ui_Obj.transform.Find("Canvas");

    }
    /// <summary>
    /// 创建并获取一个面板实例(已过时)
    /// 可选参数：面板类型的委托（事件，当面板被打开的时候）（有了同步版本后暂时不用）
    /// </summary>
    /// <param name="info"></param>
    /// <returns></returns>
    public static WaitOpenPanel<TPanel> OpenPanelByCoroutine<TPanel>(System.Action<TPanel> onPanelOpened = null) where TPanel : BasePanel, new()
    {
        GameObject parent = GameObject.Find(CanvasName);

        if (!parent)
        {
            Debug.LogError("淦！你是不是没在场景中放canvas！？");
            return null;
        }

        //panel并不作为component，只是需要一个地方存放 脚本类实例，用new不用AddComponent
        var panel = new TPanel();

        ////改成用协程方式加载
        var loadPrefabHandle = new WaitLoadAsset<GameObject>(panel.Path);

        System.Action<GameObject> openPanelAction = prefab =>
        {
            GameObject panelGobj = Object.Instantiate(prefab, parent.transform);
            //加载完面板后初始化面板脚本中的字段
            //因为 类型名就是面板名，所以可以typeof(TPanel).Name
            panelGobj.name = typeof(TPanel).Name;
            panel.Init(panelGobj);
            panel.OnOpen();
        };



        return new WaitOpenPanel<TPanel>(loadPrefabHandle, openPanelAction, onPanelOpened);
    }
    #endregion
}
