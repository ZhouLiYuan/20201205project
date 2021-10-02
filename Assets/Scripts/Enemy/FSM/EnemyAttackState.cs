using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//不能让敌人连续触发攻击方法
public class EnemyAttackState : EnemyBaseState
{
    
    En_Attacker_MeleeOverlapCircle en_atk;
   
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        en_AnimatorGobj = animator.gameObject;
      
        en_TopNodeTransform = en_AnimatorGobj.transform;
        en_HitCollider = animator.transform.GetComponent<Collider2D>();
        Init();



        //停下攻击
        en_rb2d.MovePosition(en_rb2d.position);
        //en_atk = animator.GetComponeant<En_Attacker_MeleeOverlapCircle>();
        //GetDamage dmg = GameObject.FindGameObjectWithTag("Player").GetComponent<GetDamage>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //这个值好像不放在每个State的Update里实时去算的话，好像就只会得到初始化时算的数值然后一致保持不变？
        distanceToPlayer = Vector2.Distance(playerTransform.position, en_rb2d.position);


        //敌人发动攻击
        Attack();

        //不知为何一直从EnemyConfig赋值失败
        //animator.SetFloat("AttackRange", distanceToPlayer);
        attackRange = 1.2f;
        if (distanceToPlayer > attackRange)   {animator.SetTrigger("Move"); }
      }


    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
     animator.ResetTrigger("Move");
    }
}
