using UnityEngine;

namespace Role
{
    namespace BaseEnemy
    {
        public class DamagedState : EnemyBaseState
        {

            public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
            {
                //避免层级一些旋转轴问题，之后修改层级设计
                Gobj = animator.gameObject;
                Transform = Gobj.transform;
                en_HitCollider = animator.transform.GetComponent<Collider2D>();
                Init();

                //Enemy.IsCurrentHitOver = false;
                Enemy.GetDamage();
            }


            public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
            {
                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f) { animator.SetTrigger("Idle"); }
            }

            public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
            {
                //animator.ResetTrigger("Damaged");
                animator.ResetTrigger(triggerName);
                //Enemy.IsCurrentHitOver = true;
            }

        }
    }
}