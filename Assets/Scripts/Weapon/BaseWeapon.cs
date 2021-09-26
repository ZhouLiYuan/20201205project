using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AtkType { normal, fired, poison, friendSide }
//NPC攻击用的part 命名以Attacker_开头
public class BaseWeapon
{
    public GameObject owner;

    public GameObject weaponGobj;
    public Transform transform;
    public Collider2D collider2D;

    public int ID { get; private set; }
    public string ChineseName { get; private set; }
    public float AtkValue { get; private set; }
    public int AtkType { get; private set; }
    public string Path { get; private set; }



    internal void Init(GameObject obj)
    {
        weaponGobj = obj;
        transform = weaponGobj.transform;
        collider2D = obj.AddComponent<PolygonCollider2D>();
    }

    public void InitProperties(WeaponConfig weaponConfig) 
    {
        ID = weaponConfig.ID;
        ChineseName = weaponConfig.Name;
        AtkValue = weaponConfig.Damage;
        AtkType = weaponConfig.Type;
        Path = weaponConfig.AssetPath;

    }


    public void SetOwner(GameObject owner)
    {
        this.owner = owner;
    }
}
