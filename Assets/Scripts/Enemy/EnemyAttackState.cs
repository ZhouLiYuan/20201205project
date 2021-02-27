using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : StateMachineBehaviour
{
    public int en_attackValue;
    CheckTargetToDamage_MeleeOverlapCircle en_atk;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        en_atk = animator.GetComponent<CheckTargetToDamage_MeleeOverlapCircle>();
        //GetDamage dmg = GameObject.FindGameObjectWithTag("Player").GetComponent<GetDamage>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //敌人发动攻击
        en_atk.Atk();
    }


    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
        
    //}

}
