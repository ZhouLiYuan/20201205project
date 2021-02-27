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
/// 控制enemy在移动状态下的功能：
/// 1根据玩家坐标追踪玩家
/// 2朝向面对玩家
/// 3在攻击范围内时攻击玩家
/// </summary>
public class EnemyMoveState : StateMachineBehaviour
{
    public float en_speed = 2.5f;
    public float attackRange = 3f;

    //为敌人提供追踪方位
    Transform player;


    Rigidbody2D en_rb;
    EnemyFacing enemy_facing;


    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
         player = GameObject.FindGameObjectWithTag("Player").transform;
        //这里通过anim clip所属animator获取 animator所挂载物体的刚体信息吗？
        en_rb = animator.GetComponent<Rigidbody2D>();

        //如果不在这里赋予enemy引用变量实例，下面会无法调用LookAtPlayer()方法
        enemy_facing = animator.GetComponent<EnemyFacing>();
    }


    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //修改朝向
        enemy_facing.LookAtPlayer();

        //只希望追踪敌人的横坐标，并不需要追踪Y轴，所以Y方向目标点设为自身
        Vector2 target = new Vector2(player.position.x, en_rb.position.y);
        Vector2 en_newPosition =Vector2.MoveTowards(en_rb.position, target, en_speed * Time.fixedDeltaTime);
        //实际修改敌人刚体速度的方法
        en_rb.MovePosition(en_newPosition);


        //切换为攻击模式
        if (Vector2.Distance(player.position, en_rb.position) <= attackRange) { animator.SetTrigger("Attack"); }

    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //重置trigger
        //animator.ResetTrigger("Attack");
    }


}
