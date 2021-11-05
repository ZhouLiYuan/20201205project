using System.Collections.Generic;
using UnityEngine;
using Role.SelectableRole;

namespace Role
{
    public class Enemy : RoleEntity
    {
        public EnemyInfoInspector en_infoInspector;
        
        //碰撞检测
        public DamageReceiver DamageReceiver { get; private set; }
      

        //配置表 属性
        public float speed; // 移动速度
        public float attackRange; // 攻击范围
        public float chaseRange;// 追踪范围

        //逻辑相关trigger


        //主角方位（攻击对象）
        public PlayerRole role;
        public GameObject playerGobj;
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

            DamageReceiver = obj.GetComponent<DamageReceiver>();
            //注意prefab的层级

            //动画演出en_animator

            //静态构造函数好像是在静态成员第一次被访问的时候调用
            //EnemyManager.en_nameDic.Add(this.en_name, this);
            EnemyManager.hitColliderDic.Add(HitCollider, this);

            role = PlayerManager.m_Role;
            playerGobj = PlayerManager.m_Role.GameObject;
            pl_Transform = PlayerManager.m_Role.Transform;

            distanceToPlayer = Vector2.Distance(pl_Transform.position, rg2d.position);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enemyConfig"></param>
        public void InitProperties(EnemyConfig enemyConfig)
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



        /// <summary>
        /// 让敌人自动面朝玩家
        /// 逻辑：
        ///理解为判断 相对 角色(Enemy prefab)初始朝向（朝右），是否以反转过
        ///true已反转（向左） false未反转（朝右）
        ///敌人朝向 和 玩家的朝向 无关 和 玩家方位有关
        /// 注意事项：
        /// 如果在animation里K了脚本中修改的参数帧，那么这个脚本的中的数据修改都将无效
        /// 目前解决方案，追加多个topNode用作反转，通过字Gobj修改父Gobj参数
        /// 反转的是父级的transform.localScale
        /// 教程地址https://www.youtube.com/watch?v=AD4JIXQDw0s 12分42秒
        /// </summary>
        public void LookAtPlayer()
        {

            //if语句判断 要不要把反转后的向量 赋值给en_TopNodeTransform
            Vector3 en_flip = Transform.localScale;
            en_flip.x *= -1f;

            //当Enemy在Player右边，并且Enemy还没有反转过（当前向右）
            if (Transform.position.x > playerGobj.transform.position.x && Transform.localScale.x > 0)
            { Transform.localScale = en_flip; }
            //当Enemy在Player左边，并且Enemy反转过（当前向左）
            else if (Transform.position.x < playerGobj.transform.position.x && Transform.localScale.x < 0)
            { Transform.localScale = en_flip; }
            //位置重合的情况，或其他情况
            else { }
        }

        //虚方法使得基类也有子类的同名方法，这样在BaseEnemyState的Enemy实例就可以不用泛型
        public virtual void Attack()
        {
        }

        public virtual void ChasePlayer() 
        {
        }
    }

}