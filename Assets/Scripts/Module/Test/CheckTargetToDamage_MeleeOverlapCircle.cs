using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//可以写成 CheckTargetToDamage<检测方法类型>的泛型类吗？这样是不是就可以实现所有检测方式了
public class CheckTargetToDamage_MeleeOverlapCircle : MonoBehaviour
{
    public float atkIntervalVolume;
    public float attackRange;
    public int atkValue;
    

    public LayerMask targetsLayer;
    /// <summary>
    /// 攻击范围中心点
    /// </summary>
    public Transform attackPos;

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
    /// attackRange攻击范围
    /// atkIntervalVolume 攻击间隔容量
    /// attackPos攻击范围中心点
    /// targetsLayer目标所在图层
    /// </summary>
    public void Atk(/*float attackRange, float atkIntervalVolume, Transform attackPos, LayerMask targetsLayer,int atkValue*/)
    {
        float atkInterval = 0 ;

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
                    targetsToDamage[i].GetComponent<GetDamage>().TakeDamage(atkValue);
                }

                foreach (Collider2D target in targetsToDamage)
                {
                    //为什么这段没有被打印出来？
                    //Debug.Log($" kick{target.name}'s ass!!");
                }

                //攻击结束后重置冷却时间
                atkInterval = atkIntervalVolume;
                //Debug.Log($"冷却时间重置为{atkInterval}");
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
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }


}
