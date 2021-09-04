using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//疑问
//关于Tag和对应Gobj的层级管理，可以写一套基础的伤害接受脚本
//但是到具体实现的时候恐怕还是得分开Character和Enemy两套Tag会比较管理
//完全追求通用碰撞管理上会非常混乱？


//一次只能收到一个对象的攻击
public class BaseDamageReceiver:MonoBehaviour
{
    //抽象成一个节点就可以玩家敌人共用了(而且LayerMask是可以多选的)
    public LayerMask attackerLayer;
    private DamageData data;

    //受动者(脚本拥有者) 这两个信息要不要也放在DamageData里面
    public GameObject owner;
    public LayerMask ownerLayer;
    //施动者
    public GameObject attacker;

    private void awake()
    {
       owner = data.receiver = gameObject;
       ownerLayer =data.layer = owner.layer;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        attacker = collision.gameObject;
        
        
        if (data.isInvincible = false && collision.gameObject.layer == attackerLayer) 
        {
            data = new DamageData(collision.gameObject);
 
            GlobalEvent.Damage(data);
            data.isInvincible = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        data.isInvincible = false;
        attacker = null;
    }
}
