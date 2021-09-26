using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy
{
    public /*virtual */string Path; /*{ get; }*/

    public string name;
    public GameObject en_gameObject;
    public Transform en_transform;
    public Rigidbody2D en_rb;

    //用于定位主角方位（攻击对象）
    public PlayerRole role;
    public GameObject roleGobj;
    public Transform pl_Transform;
    public Collider2D en_HitCollider;



    public int HPMax;
    public int HP;
    public int id;
    public string chineseName;



    public BaseWeapon weapon;
    public List<BaseWeapon> availableWeapons = new List<BaseWeapon>();
    public KeyValuePair<Collider2D, BaseWeapon> currentWeaponPair;

    /// <summary>
    /// 参数(scene中的具体Gobj) 初始化抽象类 字段(泛型方法中无法调用构造函数用)
    /// </summary>
    /// <param name="name"></param>
    /// <param name="obj"></param>
    internal void Init(string name, GameObject obj)
    {
        this.name = name;
        en_gameObject = obj;
        en_transform = obj.transform;
        en_rb = en_transform.GetComponent<Rigidbody2D>();
        en_HitCollider = en_transform.GetComponent<Collider2D>();

        EnemyManager.enemyDic.Add(this.name, en_gameObject);
        EnemyManager.en_colliderDic.Add(en_HitCollider, en_gameObject);


        role = PlayerManager.m_Role;
        roleGobj = PlayerManager.m_Role.GameObject;
        pl_Transform = PlayerManager.m_Role.Transform;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="enemyConfig"></param>
    public void InitProperties(EnemyConfig enemyConfig)
    {
        HP = HPMax = enemyConfig.HP;
        id = enemyConfig.ID;
        chineseName = enemyConfig.Name;
        Path = enemyConfig.AssetPath;
    }

    //要在这把weapon和enemy联系起来
    public void EquipWeapon(WeaponConfig weaponConfig)
    {
        var weapon = WeaponManager.SpawnEnemyWeapon(this, weaponConfig);


        //enemy武器切换成当前武器
        this.weapon = weapon;
        currentWeaponPair = new KeyValuePair<Collider2D, BaseWeapon>(weapon.collider2D, weapon);
    }
}
