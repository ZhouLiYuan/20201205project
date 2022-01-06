using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Attacker和DamageData里的type元素完全一致
//尝试可以不可以用int来做相等判断或者赋值
//不用写static就可以像静态成员一样去使用，详见DamageSystem
public enum AdvDamageType { normal, fired, poison, friendSide }

[System.Serializable]
public class DamageData
{
    public float atkValue;//攻击方的攻击力
    public float defValue;//受击方的防御力

    public AdvDamageType damageType = AdvDamageType.normal;//默认值

    /// <summary>
    /// 伤害接收者
    /// </summary>
    //public GameObject receiver;
    public LayerMask layer;

}
