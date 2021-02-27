using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetDamage : MonoBehaviour
{
    /// <summary>
    /// 和UI脚本HealthBar里的currentHealth通信值
    /// <summary>
    private int m_health;

    public Rigidbody2D rgbd2d;
    public HealthBar healthBar;
    public GameObject hitEffect;
    /// <summary>
    /// 原理和攻击冷却时间一样
    /// </summary>
    private float dazedTime;
    public float dazedTimeVolume;

    private Animator m_anim;
    /// <summary>
    /// destroy延时时间，死亡动画播完再死
    /// </summary>
    [SerializeField] private float deathAnimPlayTime;

    private void Start()
    {
        //UI模块
        //和HealthBar实例的currentHealth通信
        //FindAPI 只能找直属一级子物体
        m_health =healthBar.currentHealth;
        //初始化血槽
        healthBar.SetMaxHealth();

        //动画模块
        m_anim = GetComponent<Animator>();
        m_anim.SetBool("Moving", true);
    }


    /// <summary>
    /// 硬直，减慢移动速度
    /// </summary>
    public void Daze()
    {
        //硬直结束
        if (dazedTime <= 0){ }
        //硬直状态中
        else
        {
            rgbd2d.velocity *= 0;
            dazedTime -= Time.deltaTime;
        }
    }

    /// <summary>
    /// 传入收到的伤害数值
    /// </summary>
    /// <param name="getDamageValue"></param>
    public void TakeDamage(int damageValue)
    {
        
        m_anim.SetTrigger("Hurt");
        //被攻击产生硬直
        dazedTime = dazedTimeVolume;
        Daze();
        //产生被攻击特效
        //Quaternion.identity就是不旋转
        Instantiate(hitEffect, transform.position, Quaternion.identity);
        m_health -= damageValue;
        Debug.Log($"{this.name}剩余血量{m_health}");

        //与表现层通信
        healthBar.SetHealthBar(m_health);
    }

}
