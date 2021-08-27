using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 用抽象类做通用 模版
/// </summary>
public class BasePanel
{
    //代替原本的UIInfo
    public virtual string Path { get; }

    //public UIInfo UIInfo { get; private set; }
    //public UItool UItool { get; private set; }

    //public PanelManager PanelManager { get; private set; }

    //public BasePanel(UIInfo uiInfo) 
    //{
    //    UIInfo = uiInfo;
    //}

    public string m_name;
    /// <summary>
    /// 面板所在物体的 gameObject
    /// </summary>
    public GameObject m_gameObject;
    /// <summary>
    /// 面板所在物体的 transform
    /// </summary>
    public Transform m_transform;
    public CanvasGroup m_canvasGroup;

    /// <summary>
    /// 用传入的参数(scene中的具体Gobj) 初始化抽象类字段
    /// </summary>
    /// <param name="name"></param>
    /// <param name="obj"></param>
    internal void Init(string name, GameObject obj)
    {
        m_name = name;
        m_gameObject = obj;
        m_transform = obj.transform;
        m_canvasGroup = obj.AddComponent<CanvasGroup>();
    }

    public virtual void OnOpen() { }
    public virtual void OnUpdate(float deltaTime) { }
    public virtual void OnPause()
    {
        UItool.GetOrAddComponent<CanvasGroup>().blocksRaycasts = false;
    }
    public virtual void OnResume()
    {
        UItool.GetOrAddComponent<CanvasGroup>().blocksRaycasts = true;
    }
    public virtual void OnClose()
    {
        //UIManager.DestroyUI(UIInfo);
    }

    public void Close() { UIManager.ClosePanel(this); }

    public void Show() 
    {
        m_canvasGroup.alpha = 1f;
        m_canvasGroup.interactable = true;
    }
    public void Hide() 
    {
        m_canvasGroup.alpha = 0f;
        m_canvasGroup.interactable = false;
    }




    /// <summary>
    /// 提供需要找的元素的路径/名称
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <returns></returns>
    protected T Find<T>(string path) where T : Object
    {
        var t = m_transform.Find(path);
        if (typeof(T) == typeof(Transform)) return t as T;
        if (typeof(T) == typeof(GameObject)) return t.gameObject as T;
        return t.GetComponent<T>();
    }
}



