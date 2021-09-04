using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    /// <summary>
    /// 实时变化的攻击间隔
    /// 第一次攻击不用等，所以初始值为0
    /// </summary>
    private float atkInterval = 0;
    /// <summary>
    /// 攻击间隔容量，是一个常量，用于重置atkInterval
    /// 防止玩家连按，Tms的开始界面有不同的逻辑实现类似的功能
    /// </summary>
    public float atkIntervalVolume;
    public float attackRange;
    public int atkValue;

    public LayerMask enemiesLayer;
    /// <summary>
    /// 攻击范围中心点
    /// </summary>
    public Transform attackPos;



    private void Update()
    {
        //之后可以放入inputHandler
        if (Input.GetKey(KeyCode.J)) { Attack(); }  
    }

    void Attack()
    {   
        //如果想要cameraShake功能是不是要在 渲染相机上挂个animator k震动动画 然后再方法种调用？

        //当间隔冷却时间结束后可以开始下一次攻击
        if (atkInterval <= 0)
        {

            //也许这块也可做成一个 可以敌人玩家都重复利用的模块？在敌人身上实验中
            //一个CheckToAttack检测攻击范围内刚体对象
            //一个GetDamage模块

            //获取检测范围内所有敌人的碰撞体(Overlap系列)
            Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, enemiesLayer);
            //若集中对象集合非空，则对敌人造成伤害
            if (enemiesToDamage != null) 
            {
                for (int i = 0; i < enemiesToDamage.Length; i++)
                {
                    //通过collder组件 访问 所挂载Gobj的 其他组件
                    enemiesToDamage[i].GetComponent<EnemyGetDamage>().TakeDamage(atkValue);
                }

                //item名是可以随便起的吗？
                foreach (Collider2D enemy in enemiesToDamage)
                {
                    Debug.Log($" kick{enemy.name}'s ass!!");
                }

                //攻击结束后重置冷却时间
                atkInterval = atkIntervalVolume;
            }
        }
        else 
        {   //继续恢复冷却时间
            atkInterval -= Time.deltaTime;
        }
    }

    /// <summary>
    /// 可视化验证攻击区域
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
