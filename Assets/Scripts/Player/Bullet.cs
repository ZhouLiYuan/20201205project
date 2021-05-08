using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 教程地址https://www.youtube.com/watch?v=wkKsl1Mfp5M&t=0s
/// </summary>
public class Bullet : MonoBehaviour
{
    public float b_speed;
    public Rigidbody2D b_rb;

    public int b_atkValue;

    public GameObject b_hitEffect;

    void Start()
    {
        b_rb.velocity = transform.right * b_speed; 
    }

    /// <summary>
    /// 只有当两者碰撞时才会调用函数,hitTarget为被击中对象
    /// </summary>
    /// <param name="hitTarget"></param>
    private void OnTriggerEnter2D(Collider2D hitTarget)
    {
        Debug.Log(hitTarget.name);

        //检测集中对象非空（）这段代码要根据自己情况修改下！！
        BaseEnemy enemy = hitTarget.GetComponent<BaseEnemy>();
        if(enemy != null)
        {
            hitTarget.GetComponent<EnemyGetDamage>().TakeDamage(b_atkValue);
        }

        //击中特效
        Instantiate(b_hitEffect, transform.position, transform.rotation);
       
        //销毁子弹本身
        Destroy(gameObject);
    }

}
