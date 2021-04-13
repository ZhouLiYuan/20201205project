using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


/// <summary>
/// 负责从project的Resources中传参（prefab）
/// </summary>
public static class UIManager
{
    private static Transform canvasTransform;
    //new的实例只会存在在内存中，不会在场景中
    private static List<BasePanel> m_panels = new List<BasePanel>();

    //好像因为dicUI.ContainsKey(info)也不能确保面板实例的唯一性所以也被取代了？
    ///// <summary>
    ///// 配合UIInfo查询对应的Gobj
    ///// </summary>
    //private static Dictionary<UIInfo, GameObject> dicUI = new Dictionary<UIInfo, GameObject>();



    /// <summary>
    ///  协程方法初始化UI环境canvas
    /// </summary>
    /// <returns></returns>
    public static IEnumerator Init()
    {
        // 使用prefab创建已经配置好的UI环境
        var handle = Addressables.LoadAssetAsync<GameObject>("Prefab/UI/UIEnvironment.prefab暂定路径名");
        yield return handle;
        var uiPrefab = handle.Result;
        var ui_Obj = Object.Instantiate(uiPrefab);

        canvasTransform = ui_Obj.transform.Find("Canvas");

    }




    /// <summary>
    /// 获取一个 UI 实例
    /// </summary>
    /// <param name="info"></param>
    /// <returns></returns>
    public static IEnumerator OpenByCoroutine<TPanel>(TPanel panel = null) where TPanel : BasePanel, new()
    {
        GameObject parent = GameObject.Find("Canvas");

        if (!parent)
        {
            Debug.LogError("淦！你是不是没在场景中放canvas！？");
            yield return null;
        }

        //panel并不作为component，只是需要一个地方存放脚本实例，用new不用AddComponent
        panel = new TPanel();

        //每个panel类中都有对应的prefab路径信息？
        var handle = Addressables.LoadAssetAsync<GameObject>(panel.Path);
        yield return handle;
        GameObject ui_Gobj = null;

        //如果异步加载已经完成，就可以把加载到内存中的实例赋值给字段了
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            var prefab = handle.Result;
            ui_Gobj = Object.Instantiate(prefab, parent.transform);

            //加载完面板后初始化面板脚本中的字段
            //因为 类型名就是面板名，所以可以typeof(TPanel).Name
            panel.Init(typeof(TPanel).Name, ui_Gobj);
            panel.OnOpen();
            ui_Gobj.name = typeof(TPanel).Name;
        }


        panel.m_transform.SetParent(canvasTransform, false);
        //往界面 面板集合 中追加元素
        m_panels.Add(panel);


    }

    ///// <summary>
    ///// 加载 界面 时的初始化操作
    ///// </summary>
    ///// <typeparam name="Tpanel"></typeparam>
    //public static void Open<Tpanel>() where Tpanel : BasePanel, new()
    //{
    //    //获得name（需要用到的UI Prefab名称必须和 脚本中的 类名 一致）
    //    //type.Name仅仅只是把Type类型转换为了string类型,ToString也可以实现相同效果，不过type.Name更精准（因为有时不知道ToString的重载是什么）
    //    Type type = typeof(Tpanel);
    //    //var name = type.Name;
    //    var name = type.ToString();
    //    Debug.Log($"类型名称{name}");
    //    //根据 路径 名称加载 Panel UI的Prefab
    //    var prefab = Resources.Load<GameObject>($"Prefab/UI/Panel/{name}");
    //    var obj = UnityEngine.Object.Instantiate(prefab);

    //    BasePanel panel = new Tpanel();

    //    panel.Init(name,obj);
    //    panel.OnOpen();

    //    //设置父类为Canvas

    //    panel.m_transform.SetParent(canvasTransform,false);
    //    //往界面 面板集合 中追加元素
    //    m_panels.Add(panel);

    //}

    
    public static void Update() 
    {
        foreach (var panel in m_panels) 
        {
            //OnUpdate在pausePanel里被override过
            panel.OnUpdate(Time.deltaTime);
        }
    }

    public static void Close<TPanel>(TPanel panel) where TPanel : BasePanel, new()
    {
        panel.OnClose();
        //UnityEngine和.Net的System里分别各有一个Object类，需要命名空间+成员访问符UnityEngine.来访问Object
        UnityEngine.Object.Destroy(panel.m_gameObject);
        m_panels.Remove(panel);
    }

    ///// <summary>
    ///// 销毁 一个UI对象
    ///// </summary>
    ///// <param name="info"></param>
    //public static void DestroyUI(UIInfo info)
    //{
    //    GameObject.Destroy(dicUI[info]);
    //    dicUI.Remove(info);
    //}

}
