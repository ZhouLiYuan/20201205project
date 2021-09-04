using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy
{
    public virtual string Path { get; }

    public string m_name;
    public GameObject m_gameObject;
    public Transform m_transform;

    //用于定位主角方位（攻击对象）
    public GameObject player;
    public Transform playerTransform;

    public BaseAttacker attacker;

    /// <summary>
    /// 参数(scene中的具体Gobj) 初始化抽象类 字段(泛型方法中无法调用构造函数用)
    /// </summary>
    /// <param name="name"></param>
    /// <param name="obj"></param>
    internal void Init(string name, GameObject obj)
    {
        m_name = name;
        m_gameObject = obj;
        m_transform = obj.transform;

        player = PlayerManager.m_Role.GameObject;
        playerTransform = PlayerManager.m_Role.Transform;
    }
}
