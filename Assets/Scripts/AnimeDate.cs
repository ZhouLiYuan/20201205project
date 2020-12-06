using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimeDate
{
    [SerializeField] private Animator animator;
    
    private CharacterExecute animCE;
    private Movement animMove;

    void SwitchAnim()
    {
        if (animator)
        {
            animator.SetBool("grounded", animCE.m_groundCheck.grounded);
            animator.SetFloat("velocityX", Mathf.Abs(animMove.velocity.x) / animMove.maxSpeed);
        }
    }
}
