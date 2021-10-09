using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType { normal, fired, poison, friendSide }

[System.Serializable]
public class DamageData
{
    //BaseWeapon atkInfo;
    public float damageParam;
    //Attacker和DamageData里的type元素完全一致
    //尝试可以不可以用int来做相等判断或者赋值
    //不用写static就可以像静态成员一样去使用，详见DamageSystem
    public DamageType damageType = DamageType.normal;

    /// <summary>
    /// 伤害接收者
    /// </summary>
    //public GameObject receiver;
    public LayerMask layer;






}
