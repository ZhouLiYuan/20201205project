using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//NPC攻击用的part 命名以Attacker_开头
public class BaseAttacker
{
    public float atkValue;
    public enum AtkType { normal, fired, poison, friendSide }
    public AtkType atkType;

    public GameObject owner;
    public Transform transform;

    internal void Init(GameObject obj)
    {
        owner = obj;
        transform = owner.transform;
    }

}
