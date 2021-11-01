using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyDamagedState : EnemyBaseState
{
 
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //避免层级一些旋转轴问题，之后修改层级设计
        en_TopNodeGobj = animator.gameObject;
        en_TopNodeTransform = en_TopNodeGobj.transform;
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
        animator.ResetTrigger("Damaged");
        //Enemy.IsCurrentHitOver = true;
    }

}
