using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AtkType { normal, fired, poison, friendSide }
//NPC攻击用的part 命名以Attacker_开头
public class BaseWeapon
{
    public float atkValue;
    public AtkType atkType;

    public GameObject owner;
    public GameObject gameObject;
    public Transform transform;

    internal void Init(GameObject obj)
    {
        gameObject = obj;
        transform = gameObject.transform;
    }

    public void SetOwner(GameObject owner)
    {
        this.owner = owner;
    }
}
