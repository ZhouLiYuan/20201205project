using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//角色只用Init初始化，不用构造函数
public class RoleEntity : Entity
{
    public Animator animator;
    public GameObject animatorGobj;
    public Rigidbody2D rg2d;

    //需要序列化动态调节
    public int maxHP;
    private int hp;
    public int HP
    {
        get { return hp; }
        set
        {
            if (0 <= value && value <= maxHP) { hp = value; }
            else if (value < 0) { hp = 0; }
            else { hp = maxHP; }
        }
    }

    public override void Init(GameObject obj)
    {
        base.Init(obj);
        rg2d = obj.GetComponent<Rigidbody2D>();
    }

    public virtual void InitProperties(RoleConfig config)
    {
        base.InitProperties(config);
        if (config.GetType().Name == "NPCConfig") return;//暂时不给NPC设置血量
        HP = maxHP = config.HP;
    }
}