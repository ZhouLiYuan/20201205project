using UnityEngine;

namespace Role
{
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
    public class MoveBaseState : EnemyBaseState
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        { 

            en_HitCollider = animator.transform.GetComponent<Collider2D>();
            Init();
        }


        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!GroundDetect.IsGrounded) return;
            // Mathf.Clamp();可以用来做巡逻范围限定


            Enemy.ChasePlayer();

            //玩家在视线范围内
            var dir = new Vector2(Transform.localScale.x, 0);
            var result = Physics2D.Raycast(Transform.position, dir);
            if(result.collider != null)
            //animator.SetFloat("AttackRange", distanceToPlayer);
            if (distanceToPlayer <= attackRange) {animator.SetTrigger("Attack"); } //是否攻击
            if (distanceToPlayer > chaseRange) { animator.SetTrigger("Idle"); }//是否追踪
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //重置trigger
            animator.ResetTrigger("Attack");

            //停下
            Vector2 stopChasing = rg2d.velocity;
            stopChasing.x = 0f;
            rg2d.velocity = stopChasing;
        }
    }
}