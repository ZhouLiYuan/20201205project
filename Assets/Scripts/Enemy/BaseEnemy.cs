using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy
{
    public virtual string Path { get; }

    public string m_name;

    public GameObject m_gameObject;

    public Transform m_transform;


    /// <summary>
    /// 参数(scene中的具体Gobj) 初始化抽象类 字段
    /// </summary>
    /// <param name="name"></param>
    /// <param name="obj"></param>
    internal void Init(string name, GameObject obj)
    {
        this.m_name = name;
        m_gameObject = obj;
        m_transform = obj.transform;
    }
}
