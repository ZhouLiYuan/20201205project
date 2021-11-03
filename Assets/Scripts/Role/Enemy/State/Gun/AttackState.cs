﻿using Role.BaseEnemy;
using UnityEngine;

namespace Role
{
    namespace GunEnemy
    {
        //不能让敌人连续触发攻击方法
        public class AttackState : EnemyBaseState
        {

            En_Attacker_MeleeOverlapCircle en_atk;

            override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
            {
                en_HitCollider = animator.transform.GetComponent<Collider2D>();
                Init();

                //停下攻击
                en_rb2d.MovePosition(en_rb2d.position);
                Enemy.currentWeapon.collider2D.enabled = true;
            }

            override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
            {
                //这个值不放在每个State的Update里实时去算的话，只会得到初始化时算的数值后一致保持不变
                distanceToPlayer = Vector2.Distance(playerTransform.position, en_rb2d.position);


                //敌人发动攻击
                Attack();
                Enemy.currentWeapon.collider2D.enabled = true;
                //animator.SetFloat("AttackRange", distanceToPlayer);
                if (distanceToPlayer > attackRange) { animator.SetTrigger("Move"); }
            }


            override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
            {
                //animator.ResetTrigger("Attack");
                animator.ResetTrigger(triggerName);
                Enemy.currentWeapon.collider2D.enabled = false;
            }
        }
    }
}