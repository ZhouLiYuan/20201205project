using UnityEngine;

namespace Role.BaseEnemy
{
        public class DamagedState : EnemyBaseState
        {

            public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
            {

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