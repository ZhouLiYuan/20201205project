using Role.SelectableRole;
using UnityEngine;
namespace Role
{
        public class EnemyBaseState : StateMachineBehaviour
        {
            ////提供角色追踪方位
            //protected GameObject playerGobj => Enemy.roleGobj;
            //protected PlayerRole player => Enemy.role;
            //protected Transform playerTransform => Enemy.pl_Transform;


            ////敌人自身
            //public string en_instanceName => Enemy.en_name;
            ////自身字段第一层级
            //protected GameObject en_TopNodeGobj =>Enemy.en_gameObject;
            //protected Transform en_TopNodeTransform => Enemy.en_transform;
            ////自身字段第二层级
            //protected GameObject en_AnimatorGobj => Enemy.en_animatorGobj;
            //protected Rigidbody2D en_rb2d => Enemy.en_rb2d;


            ////配置表属性
            //protected float en_speed => Enemy.speed;
            //protected float attackRange => Enemy.attackRange;
            //protected float chaseRange => Enemy.chaseRange;


            ////敌人与角色关系
            //protected float distanceToPlayer => Enemy.distanceToPlayer;
            //逻辑层

            //key
            protected Collider2D en_HitCollider;
            //Instance
            protected Enemy Enemy { get; private set; }



            //提供角色追踪方位
            protected GameObject roleGobj;
            protected PlayerRole role;
            protected Transform pl_Transform;


            //敌人自身
            //自身字段第一层级
            protected GameObject Gobj;
            protected Transform Transform;
            protected Rigidbody2D rg2d;


            //地面检测
            public GroundDetect GroundDetect { get; private set; }


            //配置表属性
            protected float speed;
            protected float attackRange;
            protected float chaseRange;


            //敌人与角色关系
            protected float distanceToPlayer;

            protected string triggerName;

            protected void Init()
            {
                Enemy = EnemyManager.GetInstanceByCollider(en_HitCollider);

                roleGobj = Enemy.roleGobj;
                role = Enemy.role;
                pl_Transform = Enemy.pl_Transform;

                Gobj = Enemy.GameObject;
                Transform = Enemy.Transform;
                rg2d = Enemy.rg2d;

                GroundDetect = Enemy.GroundDetect;

                var fullName = GetType().Name;
                //var startIndex = fullName.IndexOf("y");
                //var endIndex = fullName.LastIndexOf("S");
                //triggerName = fullName.Substring(startIndex + 1, endIndex - 5);
                triggerName = fullName.Substring(0, fullName.LastIndexOf("S"));

                //配置表属性
                speed = Enemy.speed;
                attackRange = Enemy.attackRange;
                chaseRange = Enemy.chaseRange;
            }
        }

}