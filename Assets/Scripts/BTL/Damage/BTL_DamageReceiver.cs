using Role.SpineRole;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTL_DamageReceiver : MonoBehaviour
{
    public PlayerRole_Spine owner;
    public bool isAttacked = false;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (owner.InvincibleTime <= 0) 
        {
            isAttacked = true;
        }
        //还需要通过Hitbox查到当前对方出招，招式对应的伤害
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

    }
}
