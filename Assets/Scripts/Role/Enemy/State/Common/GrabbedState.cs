using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Role
{
    namespace BaseEnemy
    {
        public class GrabbedState : EnemyBaseState
        {

            public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
            {
                //避免层级一些旋转轴问题，之后修改层级设计
                Gobj = animator.gameObject;
                en_HitCollider = animator.transform.GetComponent<Collider2D>();
                Init();
                rg2d.gravityScale = 0f;//处于失重状态
                en_HitCollider.isTrigger = true;//无法与玩家产生实际碰撞
            }


            public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
            {

                if (!PlayerManager.m_Role.IsHookPressing && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f) { animator.SetTrigger("Idle"); }//被抓硬直是动画播放时长
            }

            public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
            {
                en_HitCollider.isTrigger = false;
                rg2d.gravityScale = 1f;//处于失重状态
                animator.ResetTrigger(triggerName);
                //animator.ResetTrigger("Grabbed");
                //Enemy.IsCurrentHitOver = true;
            }

        }
    }
}