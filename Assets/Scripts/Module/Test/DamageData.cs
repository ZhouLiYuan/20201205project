using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DamageData
{
    BaseWeapon atkInfo;
    public float damageParam =5f;
   
  /// <summary>
  /// 伤害接收者
  /// </summary>
    public GameObject receiver;
    public LayerMask layer;

    //无敌，避免重复刚体碰撞检测
    public bool isInvincible = false;

    //Attacker和DamageData里的type元素完全一致
    //尝试可以不可以用int来做相等判断或者赋值
    //不用写static就可以像静态成员一样去使用，详见DamageSystem
    public enum DamageType { normal, fired, poison, friendSide }
    public DamageType damageType = DamageType.normal;

    public DamageData(GameObject enemyObj)
    {
        
    }
}
