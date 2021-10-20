//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;


///// <summary>
///// 教程地址https://www.youtube.com/watch?v=1QfxdUpVh5I&t=2s  
///// 需要在同一个EnemyGobj上挂载HealthBar模块脚本
///// 对应功能：1被攻击时产生硬直（速度减慢）2生命值减少 3死亡
///// 功能1和2在玩家的攻击脚本中被调用 3在自身脚本的声明周期中调用
///// </summary>
//public class EnemyGetDamage : MonoBehaviour
//{
//    /// <summary>
//    /// 和UI脚本HealthBar里的currentHealth通信值
//    /// <summary>
//    private int en_health;

//    public GameObject hitEffect;
//    /// <summary>
//    /// 原理和攻击冷却时间一样
//    /// </summary>
//    private float dazedTime;
//    public float dazedTimeVolume;

//    public float en_speed;
//    public float en_speedNormal;

//    private Animator en_anim;
//    /// <summary>
//    /// destroy延时时间，死亡动画播完再死
//    /// </summary>
//    [SerializeField] private float deathAnimPlayTime;

//    private void Start()
//    {
//        //UI模块
//        //和HealthBar实例的currentHealth通信
//        en_health = this.GetComponent<HealthBarOld>().currentHealth;
//        //初始化血槽
//        this.GetComponent<HealthBarOld>().SetMaxHealth();

//        //动画模块
//        en_anim = GetComponent<Animator>();
//        en_anim.SetBool("moving，根据unityEditor下的参数命名", true);
//    }

//    private void Update()
//    {
//        Die();
//    }

//    /// <summary>
//    /// 敌人硬直，减慢移动速度
//    /// </summary>
//    public void Daze()
//    {
//        //硬直结束
//        if (dazedTime <= 0) { en_speed = en_speedNormal;
//; }
//        //硬直状态中
//        else
//        {
//            en_speed = 0;
//            dazedTime -= Time.deltaTime;
//        }
//    }

//    /// <summary>
//    /// 传入收到的伤害数值
//    /// </summary>
//    /// <param name="getDamageValue"></param>
//    public void TakeDamage(int damageValue)
//    {

//        en_anim.SetTrigger("Hurt");
//        //被攻击产生硬直
//        dazedTime = dazedTimeVolume;
//        Daze();
//        //产生被攻击特效
//        //Quaternion.identity就是不旋转
//        Instantiate(hitEffect, transform.position, Quaternion.identity);
//        en_health -= damageValue;
//        Debug.Log($"{this.name}敌人剩余血量{en_health}");

//        //与表现层通信
//        this.GetComponent<HealthBarOld>().SetHealthBar(en_health);
//    }

//    /// <summary>
//    /// 判断enemy是否死亡
//    /// </summary>
//    public void Die()
//    {
//        if (en_health <= 0)
//        {
//            en_anim.SetBool("IsDead", true);
//            Destroy(gameObject, deathAnimPlayTime);
//            Debug.Log($"{this.name}嗝屁了");
//            //UI血槽清零
//            this.GetComponent<HealthBarOld>().SetHealthBar(0);
//        }
//    }
//}
