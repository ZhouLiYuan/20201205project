using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


/// <summary>
/// 负责从project的Resources中传参（prefab）
/// </summary>
public static class UIManager
{
    private static Transform canvasTransform;
    //new的实例只会存在在内存中，不会在场景中
    private static List<BasePanel> m_panels = new List<BasePanel>();

    /// <summary>
    /// 配合UIInfo查询对应的Gobj
    /// </summary>
    private static Dictionary<UIInfo, GameObject> dicUI = new Dictionary<UIInfo, GameObject>();


        static UIManager()
    {
        // 使用prefab创建已经配置好的UI环境
        var uiPrefab = Resources.Load<GameObject>("Prefab/UI/UIEnvironment");
        var uiObj = UnityEngine.Object.Instantiate(uiPrefab);

        canvasTransform = uiObj.transform.Find("Canvas");
    }


    /// <summary>
    /// 获取一个 UI 实例
    /// </summary>
    /// <param name="info"></param>
    /// <returns></returns>
    public static GameObject GetSingleUI(UIInfo info)
    {
        GameObject parent = GameObject.Find("Canvas");
        
        if (!parent)
        {
            Debug.LogError("淦！你是不是没在场景中放canvas！？");
            return null;
        }
  
        if (dicUI.ContainsKey(info)) { return dicUI[info]; }


        GameObject uiGobj = GameObject.Instantiate(Resources.Load<GameObject>(info.Path), parent.transform);
        uiGobj.name = info.Name;
        dicUI.Add(info, uiGobj);
        return uiGobj;
    }


    /// <summary>
    /// 加载 界面 时的初始化操作
    /// </summary>
    /// <typeparam name="Tpanel"></typeparam>
    public static void Open<Tpanel>() where Tpanel : BasePanel, new()
    {
        //获得name（需要用到的UI Prefab名称必须和 脚本中的 类名 一致）
        //type.Name仅仅只是把Type类型转换为了string类型,ToString也可以实现相同效果，不过type.Name更精准（因为有时不知道ToString的重载是什么）
        Type type = typeof(Tpanel);
        //var name = type.Name;
        var name = type.ToString();
        Debug.Log($"类型名称{name}");
        //根据 路径 名称加载 Panel UI的Prefab
        var prefab = Resources.Load<GameObject>($"Prefab/UI/Panel/{name}");
        var obj = UnityEngine.Object.Instantiate(prefab);

        BasePanel panel = new Tpanel();

        panel.Init(name,obj);
        panel.OnOpen();
       
        //设置父类为Canvas
      
        panel.m_transform.SetParent(canvasTransform,false);
        //往界面 面板集合 中追加元素
        m_panels.Add(panel);

    }

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

    /// <summary>
    /// 销毁 一个UI对象
    /// </summary>
    /// <param name="info"></param>
    public static void DestroyUI(UIInfo info)
    {
        GameObject.Destroy(dicUI[info]);
        dicUI.Remove(info);
    }

}
