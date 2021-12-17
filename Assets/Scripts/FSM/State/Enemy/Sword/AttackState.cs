using UnityEngine;

namespace Role
{
    namespace SwordEnemy
    {
        //不能让敌人连续触发攻击方法
        public class AttackState : EnemyBaseState
        {

            override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
            {
                en_HitCollider = animator.transform.GetComponent<Collider2D>();
                Init();

                //停下攻击
                rg2d.MovePosition(rg2d.position);
                Enemy.currentWeapon.collider2D.enabled = true;
            }

            override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
            {

                //敌人发动攻击
                Enemy.Attack();
                Enemy.currentWeapon.collider2D.enabled = true;
                //animator.SetFloat("AttackRange", distanceToPlayer);
                if (distanceToPlayer > attackRange) { animator.SetTrigger("Move"); }
            }


            override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
            {
                animator.ResetTrigger("Attack");
                Enemy.currentWeapon.collider2D.enabled = false;
            }
        }
    }
}