using Role.SelectableRole;
using UnityEngine;
namespace Role
{
    public class EnemyBaseState : StateMachineBehaviour
    {
        ////提供角色追踪方位
        //protected GameObject playerGobj => Enemy.roleGobj;
        // =>属性的get方法只有在属性被访问时才会调用
        //实例为空也有可能是StateMachineBehaviour的OnStateEnter执行时机比mono初始化Enemy实例还早
        // 但如果设想正确的画，应该在Enemy = EnemyManager.GetInstanceByCollider(en_HitCollider);时报错
        //而且mono执行时机是比StateMachineBehaviour早的
        //20211113这个重新解除注释修改了一些代码的位置，这个问题被莫名其妙地解决了。。。。。（难道是层级变成同一层的缘故？？？）

        protected GameObject playerGobj => Enemy.playerGobj;
        protected PlayerRole role => Enemy.role;
        protected Transform pl_Transform => Enemy.pl_Transform;


        //敌人自身
        public string en_instanceName => Enemy.UniqueName;
        //自身字段第一层级
        protected GameObject Gobj => Enemy.GameObject;
        protected Transform Transform => Enemy.Transform;
        protected Rigidbody2D rg2d => Enemy.rg2d;

        protected GroundDetect GroundDetect => Enemy.GroundDetect;

        //配置表属性
        protected float speed => Enemy.speed;
        protected float attackRange => Enemy.attackRange;
        protected float chaseRange => Enemy.chaseRange;


        //敌人与角色关系
        protected float distanceToPlayer => Enemy.distanceToPlayer;
        //逻辑层

        //key
        protected Collider2D en_HitCollider;
        //Instance
        protected Enemy Enemy { get; private set; }



        ////提供角色追踪方位
        //protected GameObject roleGobj;
        //protected PlayerRole role;
        //protected Transform pl_Transform;


        ////敌人自身
        ////自身字段第一层级
        //protected GameObject Gobj;
        //protected Transform Transform;
        //protected Rigidbody2D rg2d;


        ////地面检测
        //public GroundDetect GroundDetect { get; private set; }


        ////配置表属性
        //protected float speed;
        //protected float attackRange;
        //protected float chaseRange;


        ////敌人与角色关系
        //protected float distanceToPlayer;

        protected string triggerName;

        protected void Init()
        {
            Enemy = EnemyManager.GetInstanceByCollider(en_HitCollider);

            //roleGobj = Enemy.playerGobj;
            //role = Enemy.role;
            //pl_Transform = Enemy.pl_Transform;

            //Gobj = Enemy.GameObject;
            //Transform = Enemy.Transform;
            //rg2d = Enemy.rg2d;

            //GroundDetect = Enemy.GroundDetect;

            var fullName = GetType().Name;
            //var startIndex = fullName.IndexOf("y");
            //var endIndex = fullName.LastIndexOf("S");
            //triggerName = fullName.Substring(startIndex + 1, endIndex - 5);
            triggerName = fullName.Substring(0, fullName.LastIndexOf("S"));

            //配置表属性
            //speed = Enemy.speed;
            //attackRange = Enemy.attackRange;
            //chaseRange = Enemy.chaseRange;
        }
    }

}