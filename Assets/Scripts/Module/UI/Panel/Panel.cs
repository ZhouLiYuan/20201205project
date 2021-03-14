using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 用抽象类做通用 模版
/// </summary>
public class Panel
{
    public string m_name;
    /// <summary>
    /// 面板所在物体的 gameObject
    /// </summary>
    public GameObject m_gameObject;
    /// <summary>
    /// 面板所在物体的 transform
    /// </summary>
    public Transform m_transform;

    //初始化方法  为什么是定义为程序集合内可访问？
    /// <summary>
    /// 用传入的参数(scene中的具体Gobj) 初始化抽象类字段
    /// </summary>
    /// <param name="name"></param>
    /// <param name="obj"></param>
    internal void Init(string name, GameObject obj)
    {
        this.m_name = name;
        m_gameObject = obj;
        m_transform = obj.transform;
    }

    public virtual void OnOpen() { }
    public virtual void OnUpdate(float deltaTime) { }
    public virtual void OnClose() { }

}
