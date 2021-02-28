using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : StateMachineBehaviour
{
    public int en_attackValue;
    CheckTargetToDamage_MeleeOverlapCircle en_atk;

    Transform player;
    Rigidbody2D en_rb;
    EnemyMoveState ems;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        en_rb = animator.GetComponent<Rigidbody2D>();
        ems = new EnemyMoveState();

        en_atk = animator.GetComponent<CheckTargetToDamage_MeleeOverlapCircle>();
        //GetDamage dmg = GameObject.FindGameObjectWithTag("Player").GetComponent<GetDamage>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //敌人发动攻击
        en_atk.Atk();
        if (Vector2.Distance(player.position, en_rb.position) > ems.attackRange)
        {
            animator.SetTrigger("Move");
        }
    }


    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

       
    }

}
