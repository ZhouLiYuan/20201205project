using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyGrabbedState : EnemyBaseState
{
 
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //避免层级一些旋转轴问题，之后修改层级设计
        en_AnimatorGobj = animator.gameObject;
        en_TopNodeTransform = en_AnimatorGobj.transform;
        en_HitCollider = animator.transform.GetComponent<Collider2D>();
        Init();
    }


    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!PlayerManager.m_Role.IsHookPressed && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f) { animator.SetTrigger("Idle"); }//被抓硬直是动画播放时长
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //animator.ResetTrigger(triggerName);
        animator.ResetTrigger("Grabbed");
        //Enemy.IsCurrentHitOver = true;
    }

}
