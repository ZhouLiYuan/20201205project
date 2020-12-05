using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimeDate
{
    [SerializeField] private Animator animator;
    //animator是一个布尔类型数据？

    void SwitchAnim()
    {
        if (animator)
        {
            animator.SetBool("grounded", grounded);
            animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);
        }
    }
}
