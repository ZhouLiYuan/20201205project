using UnityEngine;

namespace Role
{
    namespace GunEnemy
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
                Enemy.currentWeapon.collider2D.enabled = true;//如果枪支本身碰撞敌人也算的话
               Enemy.animEvent.AttackEvent += Enemy.Attack;
            }

            override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
            {
                //这个值不放在每个State的Update里实时去算的话，只会得到初始化时算的数值后一致保持不变
                distanceToPlayer = Vector2.Distance(pl_Transform.position, rg2d.position);
                //敌人发动攻击
                //if (stateInfo.normalizedTime > 1f) { animator.SetTrigger("Move"); }
                if (distanceToPlayer > attackRange) { animator.SetTrigger("Move"); }
            }


            override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
            {
                //animator.ResetTrigger("Attack");
                animator.ResetTrigger(triggerName);
                Enemy.currentWeapon.collider2D.enabled = false;
                Enemy.animEvent.AttackEvent -= Enemy.Attack;//防止单个方法被重复添加
            }
        }
    }
}