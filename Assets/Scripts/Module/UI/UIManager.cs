﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


/// <summary>
/// 负责UI的管理，加载和生成
/// </summary>
public static class UIManager
{
    public static Transform canvasTransform { get; private set; }
    public static Camera UICamera { get; private set; }
    //new的实例只会存在在内存中，不会在场景中
    private static List<BasePanel> m_panels = new List<BasePanel>();

    static UIManager()
    {
        // prefab创建UI环境Gobj
        var handle = Addressables.LoadAssetAsync<GameObject>("UIEnvironment");
        var uiPrefab = handle.WaitForCompletion();
        var ui_Obj = Object.Instantiate(uiPrefab);

        //建立表现层和逻辑层之间关系
        canvasTransform = ui_Obj.transform.Find("Canvas");
        UICamera = ui_Obj.transform.Find("UICamera").GetComponent<Camera>();
    }
    /// <summary>
    ///  创建并获取一个面板实例(同步加载方法)
    /// </summary>
    /// <typeparam name="TPanel"></typeparam>
    /// <returns></returns>
    public static TPanel OpenPanel<TPanel>(System.Action<TPanel> onPanelOpened = null) where TPanel : BasePanel, new() 
    {
        //panel脚本实例
        var panel = new TPanel();

        //panelGobj实例(先加载后创建)
        var prefab = AssetModule.LoadAsset<GameObject>(panel.Path);
        GameObject panelGobj = Object.Instantiate(prefab,canvasTransform);

        panelGobj.name = typeof(TPanel).Name;
        Debug.Log($"类型名称{panelGobj.name}");
        //初始化panel脚本字段，调用打开时的回调方法
        panel.Init(panelGobj.name, panelGobj);
        panel.OnOpen();

        return panel;
    }

    //和上面的OpenPanel<TPanel>写法略有不同，除了上面有可选参数可提供，其他没有差别
    public static TPanel Open<TPanel>() where TPanel : BasePanel, new()
    {
        TPanel panel = new TPanel();

        //根据 路径 名称加载 Panel UI的Prefab
        var prefab = AssetModule.LoadAsset<GameObject>(panel.Path);
        //var obj = Object.Instantiate(prefab, canvasTransform);
        var obj = Object.Instantiate(prefab);
        Object.DontDestroyOnLoad(obj);

        //获得name（需要用到的UI Prefab名称必须和 脚本中的 类名 一致）
        //type.Name仅仅只是把Type类型转换为了string类型
        //ToString也可以实现相同效果，不过type.Name更精准（因为有时不知道ToString的重载是什么）
        System.Type type = typeof(TPanel);
        var name = type.ToString();
        Debug.Log($"类型名称{name}");
        panel.Init(name, obj);
        panel.OnOpen();

        //设置父类为Canvas

        panel.m_transform.SetParent(canvasTransform, false);
        //往界面 面板集合 中追加元素
        m_panels.Add(panel);
        return panel;
    }


    /// <summary>
    /// 刷新面板信息
    /// </summary>
    public static void OnUpdate()
    {
        foreach (var panel in m_panels)
        {
            //OnUpdate在pausePanel里被override过
            panel.OnUpdate(Time.deltaTime);
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
        UnityEngine.Object.Destroy(panel.m_gameObject);
        m_panels.Remove(panel);
    }




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

        canvasTransform = ui_Obj.transform.Find("Canvas");

    }
    /// <summary>
    /// 创建并获取一个面板实例
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
