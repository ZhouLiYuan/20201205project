﻿using UnityEngine;

/// <summary>
/// 建立每个单体Gobj（表现层）和脚本（逻辑层）之间的联系
/// </summary>
public class Entity
{
    public GameObject GameObject { get; protected set;}
    public Transform Transform { get; protected set;}

    internal Entity(GameObject obj) 
    {
        this.GameObject = obj;
        this.Transform = obj.transform;
    }
}
