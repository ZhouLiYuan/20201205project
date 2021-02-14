using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapon;

//教程https://www.youtube.com/watch?v=AD4JIXQDw0s 15分40秒
public class SwordScript : LightMelee
{
    public float attackDamage;
    public float attackRange;

    //击退效果
    public Vector3 attackOffset;
    public LayerMask attackMask;

    public void Attack() 
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Collider2D colInfo = Physics2D.OverlapCircle(pos, attackRange, attackMask);
        if (colInfo != null)
        {
            //colInfo.GetComponent<PlayerHealth>().TakeDamage(attackDamage);
        }
    }


    void OnDrawGizmosSelected()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Gizmos.DrawWireSphere(pos, attackRange);
    }


}
