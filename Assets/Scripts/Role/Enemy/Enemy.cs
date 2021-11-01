using System.Collections.Generic;
using UnityEngine;

public class Enemy : RoleEntity
{
  
    public GroundDetect GroundDetect { get; private set; }
    public EnemyInfoInspector en_infoInspector;


    //配置表 属性

    public float speed; // 移动速度
    public float attackRange; // 攻击范围
    public float chaseRange;// 追踪范围

    //逻辑相关trigger


    //主角方位（攻击对象）
    public PlayerRole role;
    public GameObject roleGobj;
    public Transform pl_Transform;


    //敌人与角色关系
    public float distanceToPlayer;

    //一些逻辑判断
    //玩家虽可连击但每次攻击只能触发一次伤害
    public bool IsCurrentHitOver = true;


    //敌人当前装备的武器
    public BaseWeapon currentWeapon;
    //public List<BaseWeapon> availableWeapons = new List<BaseWeapon>();
    //public KeyValuePair<Collider2D, BaseWeapon> currentWeaponPair;
    //Collider是武器的
    public Dictionary<Collider2D, BaseWeapon> availableWeapons = new Dictionary<Collider2D, BaseWeapon>();
  

    //事件
    public event System.Action<GameObject> OnAttack;
    public void Attack(GameObject target) { OnAttack?.Invoke(target); }


    /// <summary>
    /// 参数(scene中的具体Gobj) 初始化抽象类 字段(泛型方法中无法调用构造函数用)
    /// </summary>
    /// <param name="name"></param>
    /// <param name="obj"></param>
    public override void Init(GameObject obj)
    {
        //第一层级
        base.Init(obj);
        en_infoInspector = GameObject.AddComponent<EnemyInfoInspector>();  

       //自身字段第二层级
       GroundDetect = obj.GetComponentInChildren<GroundDetect>();
        //注意prefab的层级
        
        //动画演出en_animator

        //静态构造函数好像是在静态成员第一次被访问的时候调用
        //EnemyManager.en_nameDic.Add(this.en_name, this);
        EnemyManager.hitColliderDic.Add(HitCollider, this);

        role = PlayerManager.m_Role;
        roleGobj = PlayerManager.m_Role.GameObject;
        pl_Transform = PlayerManager.m_Role.Transform;

        distanceToPlayer = Vector2.Distance(pl_Transform.position, rg2d.position);

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="enemyConfig"></param>
    public  void InitProperties(EnemyConfig enemyConfig)
    {
        base.InitProperties(enemyConfig);
        speed = enemyConfig.Speed;
        attackRange = enemyConfig.AttackRange;
        chaseRange = enemyConfig.ChaseRange;

        en_infoInspector.AssetID = enemyConfig.AssetID;
    }


    //要在这把weapon和enemy联系起来
    public void EquipWeapon(WeaponConfig weaponConfig)
    {
        var weapon = WeaponManager.SpawnEnemyWeapon(this, weaponConfig);

        //enemy武器切换成当前武器
        this.currentWeapon = weapon;
        en_infoInspector.WeaponID = weaponConfig.AssetID;
        //currentWeaponPair = new KeyValuePair<Collider2D, BaseWeapon>(weapon.collider2D, weapon);
    }

    public void GetDamage() 
    {
        var attacker = PlayerManager.m_Role;
        var data = attacker.currentWeapon.ATK();
        var finalDamageValue = DamageSystem.CalculateDamage(data);
        HP -= finalDamageValue;
    }
}
