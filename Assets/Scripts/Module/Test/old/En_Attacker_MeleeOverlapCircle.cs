using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////下面是brackey定位武器中心点的方式
//public Vector3 attackOffset;
//public void Attack() 
//{

//    Vector3 atkPos = transform.position;
//    // transform.right 指的应该是 操作器（选中状态下的w键）红色轴的默认指向？
//    atkPos += transform.right * attackOffset.x;
//    atkPos += transform.up * attackOffset.y;
//}

/// <summary>
/// 对应每个攻击武器的逻辑层
/// </summary>
public class En_Attacker_MeleeOverlapCircle : BaseWeapon
{
    /// 攻击范围中心点
    public Transform attackPos;
    //attackRange检测为切换攻击状态的范围（攻击的范围则是collider的运动面积范围）
    public float attackRange;

    ////攻击间隔
    //public float atkInterval;
 
   //可以抽象出来作为一个节点
    public LayerMask targetsLayer;

    
    //在 播放攻击动画时 同步 调用该方法
    public void Atk()
    {

        ////获取检测范围内所有敌人的碰撞体(Overlap系列)
        //Collider2D targetToDamage = Physics2D.OverlapCircle(attackPos.position, attackRange, targetsLayer);
        ////若集中对象集合非空，则对敌人造成伤害
        //if (targetToDamage != null)
        //{
        //    BaseEnemy 被打的人 = EnemyManager.通过COllider2D找到Enemy[targetToDamage];

        //    for (int i = 0; i < 所有单位; i++)
        //    {
        //        BaseEnemy enemy;
        //        if (enemy.HitCollider == targetToDamage)
        //        {
        //            GlobalEvent.Damage(new DamageData() { });
        //            break;
        //        }
        //    }

        //    targetToDamage.GetComponent<GetDamage>().TakeDamage(atkValue);
        //    Debug.Log($" kick's ass!!");

        //    }
        }

    /// <summary>
    /// 可视化验证攻击区域
    /// </summary>
    //void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(attackPos.position, attackRange);
    //}


}
