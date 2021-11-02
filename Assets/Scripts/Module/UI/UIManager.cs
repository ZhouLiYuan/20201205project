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

    public static Transform CanvasTransform { get; private set; }
    //public static Transform LocalcanvasTransform;
    //想单独做一个随心移动的canvas可以参考HealthBarOld的位置设置方式
    public static Camera UICamera { get; private set; }


    //new的实例只会存在在内存中，不会在场景中
    private static List<BasePanel> m_panels = new List<BasePanel>();
    private static List<GameObject> m_guiders = new List<GameObject>();

  

    static UIManager()
    {
        // prefab创建UI环境Gobj
        var handle = Addressables.LoadAssetAsync<GameObject>("UIEnvironment");
        var uiPrefab = handle.WaitForCompletion();
        var ui_Obj = Object.Instantiate(uiPrefab);

        //建立表现层和逻辑层之间关系
        CanvasTransform = ui_Obj.transform.Find("Canvas");
        UICamera = ui_Obj.transform.Find("UICamera").GetComponent<Camera>();
        UICamera.depth = 99;
    }
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
        //panelGobj实例(先加载后创建)
        var prefab = ResourcesLoader.LoadPanelPrefab(name);
        GameObject panelGobj = Object.Instantiate(prefab,CanvasTransform);

        Debug.Log($"类型名称{name}");
        panel.Init(panelGobj.name, panelGobj);
        panel.OnOpen();

        return panel;
    }

    //和上面的OpenPanel<TPanel>写法略有不同，除了上面有可选参数可提供，其他没有差别
    public static TPanel Open<TPanel>() where TPanel : BasePanel, new()
    {
        TPanel panel = new TPanel();
        //获得name（UI Prefab名必须和 脚本 类名 一致）
        //type.Name仅仅只是把Type类型转换为了string类型
        //ToString也可以实现相同效果，不过type.Name更精准（因为有时不明确ToString的重载）
        System.Type type = typeof(TPanel);
        var name = type.ToString();
        Debug.Log($"类型名称{name}");

        var prefab = ResourcesLoader.LoadPanelPrefab(name);
        var obj = Object.Instantiate(prefab, CanvasTransform);
        obj.name = name;
        Object.DontDestroyOnLoad(obj);

        panel.Init(name, obj);
        panel.OnOpen();

        //设置父类为Canvas
        panel.m_transform.SetParent(CanvasTransform, false);
        //往界面 面板集合 中追加元素
        m_panels.Add(panel);
        return panel;
    }


    /// <summary>
    /// 刷新面板信息
    /// </summary>
    public static void OnUpdate()
    {
        foreach (var panel in m_panels)panel.OnUpdate(Time.deltaTime);
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
        UnityEngine.Object.Destroy(panel.m_gameObject);
        m_panels.Remove(panel);
    }
 

    /// <summary>
    /// 在对象的略微上方生成
    /// </summary>
    /// <param name="name"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    public static GameObject SpawnGuiderUI(string name,Transform parent)  
    {
        var offset = 1f;
        var guider = ResourcesLoader.LoadGuiderPrefab(name);
        var Gobj = Object.Instantiate(guider,parent,false);
        Gobj.transform.position = new Vector3(Gobj.transform.position.x, Gobj.transform.position.y+offset, Gobj.transform.position.z); 
        m_guiders.Add(Gobj);
        return Gobj;
    }

    public static void DestoryGuiderUI(GameObject obj)
    {
        m_guiders.Remove(obj);
        Object.Destroy(obj);
    }

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
        GamePanel.HealthBarNameDic[owner.UniqueName] = healthBar;
        return healthBar;
        //希望UI类方法可以写的更通用
    }





    /// <summary>
    /// 能换算普通Transform为Rect Transform？
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
    /// 有offset的重载
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



    public static Color GetRandomColor()
    {
        float r = Random.Range(0f, 1f);
        float g = Random.Range(0f, 1f);
        float b = Random.Range(0f, 1f);
        var color = new Color(r, g, b, 1f);
        return color;
    }


    //-----------------------------<过时方法>---------------------------------
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
        GameObject parent = GameObject.Find("Canvas");

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
             panel.Init(panelGobj.name, panelGobj);
             panel.OnOpen();
         };



        return new WaitOpenPanel<TPanel>(loadPrefabHandle, openPanelAction, onPanelOpened);
    }
}
