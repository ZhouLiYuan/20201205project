using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//教程地址https://www.youtube.com/watch?v=AD4JIXQDw0s
//继承自 状态机 基类
//继承自状态机基类的脚本只能挂载在anim clip下
//OnStateEnter在进入动画状态的时候调用
//OnStateUpdate每一帧都会调用
//OnStateExit退出动画时调用

//这些回调函数的 参数 是在 动画播放的时候Unity自动传入的吗

/// <summary>
/// 子物体修改动画，父物体修改实际移动
/// 控制enemy在移动状态下的功能：
/// 1根据玩家坐标追踪玩家
/// 2朝向面对玩家
/// 3在攻击范围内时攻击玩家
/// </summary>
public class EnemyMoveState : EnemyBaseState
{
    public float en_speed = 2.5f;
    public float attackRange = 3f;

    EnemyFacing enemy_facing;


    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Init();
        //通过anim clip所属animator获取 animator所挂载物体的刚体信息
        en_parent_rb = animator.transform.parent.GetComponent<Rigidbody2D>();
        //player = GameObject.FindGameObjectWithTag("Player").transform;

        enemy_facing = new EnemyFacing(animator.gameObject);
    }


    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //修改朝向
        enemy_facing.LookAtPlayer();

        //只希望追踪player的横坐标，并不需要追踪Y轴，所以Y方向目标点设为自身
        Vector2 targetPosition = new Vector2(playerTransform.position.x, en_parent_rb.position.y);
        //en_newPosition只是enemy需要移动到的目标矢量Pos，而不是enemy刚体本身
        Vector2 en_newPosition =Vector2.MoveTowards(en_parent_rb.position, targetPosition, en_speed * Time.fixedDeltaTime);
        //实际修改敌人刚体矢量
        en_parent_rb.MovePosition(en_newPosition);
     
        //切换为攻击模式
        if (Vector2.Distance(playerTransform.position, en_parent_rb.position) <= attackRange)
        {
            animator.SetTrigger("Attack");
            //停下来攻击
            en_parent_rb.MovePosition(en_parent_rb.position);
        }
      
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //重置trigger
        animator.ResetTrigger("Attack");
    }


}
