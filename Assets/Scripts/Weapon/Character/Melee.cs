using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee:BaseWeapon
{
    /// 攻击范围中心点
    public Transform attackPos;
    //attackRange检测为切换攻击状态的范围（攻击的范围则是collider的运动面积范围）
    public float attackRange;

    ////攻击间隔
    //public float atkInterval;

    public LayerMask targetsLayer;




    public void Atk()
    {
        float atkInterval = 0;

        if (atkInterval <= 0)
        {
            //获取检测范围内所有敌人的碰撞体(Overlap系列)
            Collider2D[] targetsToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, targetsLayer);
            //若集中对象集合非空，则对敌人造成伤害
            if (targetsToDamage != null)
            {
                for (int i = 0; i < targetsToDamage.Length; i++)
                {
                    //通过collder组件 访问 所挂载Gobj的 其他组件
                    //targetsToDamage[i].GetComponent<GetDamage>().TakeDamage(atkValue);
                }

                foreach (Collider2D target in targetsToDamage)
                {
                    //为什么这段没有被打印出来？
                    //Debug.Log($" kick{target.name}'s ass!!");
                }

                //攻击结束后重置冷却时间
                //atkInterval = this.atkInterval;
                //Debug.Log($"冷却时间重置为{atkInterval}");
            }
        }
        //else
        //{   //继续恢复冷却时间
        //    atkInterval -= Time.deltaTime;
        //}
    }

    /// <summary>
    /// 可视化验证攻击区域
    /// </summary>
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }

}
