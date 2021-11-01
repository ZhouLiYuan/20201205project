using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AtkType { normal, fired, poison, friendSide }

//NPC攻击用的part 命名以Attacker_开头
public class BaseWeapon:Entity
{
    public GameObject owner;
    public Collider2D collider2D;

    public float AtkValue { get; private set; }
    public int AtkType { get; private set; }


    public override void Init(GameObject obj)
    {
        base.Init(obj);
        collider2D = obj.AddComponent<PolygonCollider2D>();
        collider2D.enabled = false;
        collider2D.isTrigger = true;
    }

    public void InitProperties(WeaponConfig config) 
    {
        base.InitProperties(config);
        AtkValue = config.Damage;
        AtkType = config.DamageType;

        //为了动画层级(所以没有UniqueName)，不适合NameDic
        GameObject.name = config.AssetName;
    }


    public void SetOwner(GameObject owner)
    {
        this.owner = owner;
    }

 
    public virtual DamageData ATK() 
    {
        var data = new DamageData();
        data.atkValue = AtkValue;
        data.damageType = (DamageType)AtkType;
        return data;
    }
}
