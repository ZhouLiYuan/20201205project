using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy/* : Entity*/
{

    //实例的名字
    public string en_name;
    public GameObject en_animatorGobj;
    public GameObject en_gameObject;
    public Transform en_topNodeTransform;
    public Animator en_animator;
    public Rigidbody2D en_rb2d;
    public Collider2D en_HitCollider;
    public GroundDetect GroundDetect { get; private set; }
    public EnemyInfoInspector en_infoInspector;


    //配置表 属性

    public int id;
    public string path;
    
    public int maxHP;
    private int hp;
    public int HP// 生命值
    {
        get { return hp; }
        set
        {
            if (0 <= value && value <= maxHP) { hp = value; }
            else if (value < 0) { hp = 0; }
            else { hp = maxHP; }
        }
    }


    public string assetName;
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
    internal void Init(GameObject obj)
    {
        //自身字段第一层级
        en_name = obj.name;
        en_gameObject = obj;
        en_topNodeTransform = obj.transform;

        en_infoInspector = en_gameObject.AddComponent<EnemyInfoInspector>();  

       //程序物理计算
       en_rb2d = obj.GetComponent<Rigidbody2D>();
       


       //自身字段第二层级
       GroundDetect = obj.GetComponentInChildren<GroundDetect>();
        en_animatorGobj = Find<GameObject>("animator_top");
        //注意prefab的层级
        en_HitCollider = Find<Collider2D>("animator_top");
        //动画演出en_animator
        en_animator = Find<Animator>("animator_top");

        //静态构造函数好像是在静态成员第一次被访问的时候调用
        //EnemyManager.en_nameDic.Add(this.en_name, this);
        EnemyManager.en_hitColliderDic.Add(en_HitCollider, this);

        role = PlayerManager.m_Role;
        roleGobj = PlayerManager.m_Role.GameObject;
        pl_Transform = PlayerManager.m_Role.Transform;

        distanceToPlayer = Vector2.Distance(pl_Transform.position, en_rb2d.position);

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="enemyConfig"></param>
    public void InitProperties(EnemyConfig enemyConfig)
    {
        HP = maxHP = enemyConfig.HP;
        id = enemyConfig.AssetID;
        assetName = enemyConfig.AssetName;
        speed = enemyConfig.Speed;
        attackRange = enemyConfig.AttackRange;
        chaseRange = enemyConfig.ChaseRange;

        en_infoInspector.AssetID = enemyConfig.AssetID;
    }

    /// <summary>
    /// 只能查找子物体，以及子物体的component
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <returns></returns>
    public T Find<T>(string path) where T : Object
    {
        var t = en_topNodeTransform.Find(path);
        if (typeof(T) == typeof(Transform)) return t as T;
        if (typeof(T) == typeof(GameObject)) return t.gameObject as T;
        return t.GetComponent<T>();
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
