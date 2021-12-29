using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Role.SelectableRole
{
    //只包括基本的移动操作
    public class ControllableRole<TPlayerInput> : RoleEntity where TPlayerInput : PlayerInput,new()
    {
        #region 输入trigger
        public TPlayerInput playerInput;
        public Vector2 inputAxis;
        public bool IsJumpPressed { get; protected set; }
        #endregion

        #region 移动相关
        //逻辑trigger
        public bool canApplyGravity = true;
        public bool canMoveHorizontal = true;

        public Vector2 Velocity//刚体速度
        {
            get { return rg2d.velocity; }
            set { rg2d.velocity = value; }
        } 

        //物理
        public float jumpSpeed = 10f;
        public float moveSpeed = 10f;

        //重力
        /// <summary>
        /// 代指y轴速度最大可加速到maxGravity 
        /// </summary>
        protected float maxGravity = -10f;
        protected float gravity = 9.8f;
        #endregion

        public override void Init(GameObject obj)
        {
            //第一层级
            base.Init(obj);
        }

        /// <summary>
        /// 安排每个Action会回调的方法（），具体修改rigidbody velocity之类的应该放在对应的State类里
        /// </summary>
        /// <param name="inputHandler"></param>
        public virtual void BindInput(TPlayerInput inputHandler)
        {
            //对照
            //按下GetKeyDown----Performed(不希望连按判定)
            //按住GetKey----Started(按住包含了按下瞬间Performed）
            //松开GetKeyUp----Canceled

            //换角色时 解绑输入需要用到playerInput 这个引用缓存
            this.playerInput = inputHandler;
            //move(实际物理输入的值) 
            playerInput.Move.performed += context => inputAxis = context.ReadValue<Vector2>();
            playerInput.Move.started += context => inputAxis = context.ReadValue<Vector2>();
            playerInput.Move.canceled += context => inputAxis = Vector2.zero;

            playerInput.Jump.started += context => IsJumpPressed = true;
            playerInput.Jump.canceled += context => IsJumpPressed = false;
        }

        protected void ApplyGravity(float fixedDeltaTime)
        {
            //Mathf.Max返回两个指定数字中较大
            Velocity = new Vector2(Velocity.x, Mathf.Max(Velocity.y - fixedDeltaTime * gravity, maxGravity));
        }

        protected override void OnFixedUpdate(float fixedDeltaTime)
        {
            base.OnFixedUpdate(fixedDeltaTime);
            if (canApplyGravity) ApplyGravity(fixedDeltaTime);
        }
    }
}