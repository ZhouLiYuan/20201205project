using System;
using System.Collections.Generic;
using UnityEngine;

namespace Role
{
    //角色只用Init初始化，不用构造函数
    public class RoleEntity : Entity
    {
        public Collider2D HitCollider;
        public Rigidbody2D rg2d;
        public Animator animator;
        public AnimEventCollection animEvent;

        protected Updater updater;

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

        //一些碰撞检测脚本
        public GroundDetect GroundDetect { get; private set; } //地面检测
  


        public override void Init(GameObject roleGobj)
        {
            base.Init(roleGobj);
            HitCollider = Transform.GetComponent<BoxCollider2D>();//目前角色身上唯一的BoxCollider作为受伤检测刚体
            animator = Transform.GetComponent<Animator>();
            animEvent = GameObject.GetComponent<AnimEventCollection>();
            //程序物理计算
            rg2d = roleGobj.GetComponent<Rigidbody2D>();
            GroundDetect = roleGobj.GetComponentInChildren<GroundDetect>();

            //updater相关  
            //为场景中叫Updater的Gobj添加逻辑层Updater组件
            updater = Updater.AddUpdater(roleGobj);
            //单一方法 作为 一个Action参数传入Action集合（之后再集中调用）
            updater.AddUpdateFunction(OnUpdate);
            updater.AddFixedUpdateFunction(OnFixedUpdate);

            InitFSM();
        }

        public virtual void InitProperties(RoleConfig config)
        {
            base.InitProperties(config);
            if (config.GetType().Name == "NPCConfig") return;//暂时不给NPC设置血量

            //连等可能有风险
            HP = maxHP = config.HP;
        }
        protected virtual void InitFSM() { }
        protected virtual void OnUpdate(float deltaTime) { }
        protected virtual void OnFixedUpdate(float fixedDeltaTime) { }
    }


}


