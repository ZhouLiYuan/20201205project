using System.Collections;
using UnityEngine;
using UnityEngine.Animations;


//分层的FSM是同时进行的
//只能转换到Add进FSM里的State


namespace Role
{
    namespace SelectableRole
    {
        /// <summary>
        /// 角色状态 
        /// 由于FSM分层，修改某state实例的字段无法同步到其他同名实例State
        /// 所以字段的set应统一到PlayerRole
        /// </summary>
        public class PlayerRoleState : State
        {
            public PlayerRole Role { get; private set; }
            protected GameObject role_Gobj => Role.GameObject;

            //一些输入
            protected Vector2 InputAxis => Role.inputAxis;
            protected float MoveSpeed => Role.moveSpeed;
            protected float JumpSpeed => Role.jumpSpeed;

            //动画相关部分
            protected string triggerName;
            protected string animClipName;
            protected int animLayer = 0;
            protected AnimatorStateInfo animInfo;
            protected AnimationClip currentAnim;
            protected int currentAnimFrame;
            protected float AnimDeltaTime;//从当前state动画播放开始所经过的时间

            protected GroundDetect GroundDetect => Role.GroundDetect;

            //可set
            protected Animator Animator
            {
                get { return Role.animator; }
                set { Role.animator = value; }
            }
            protected Vector2 Velocity
            {
                get { return Role.Velocity; }
                set { Role.Velocity = value; }
            }

            public void SetPlayerRole(PlayerRole role)
            {
                Role = role;
                Init();
            }

            /// <summary>
            ///  Animator.SetTrigger($"{triggerName}");
            /// </summary>
            public void Init()
            {
                //会自动获得当前 实现类而不是基类的实例
                var fullName = GetType().Name;
                triggerName = fullName.Substring(0, fullName.LastIndexOf("S"));
                animClipName = triggerName;
                //在每个状态出入的时候可以  Animator.SetTrigger($"{triggerName}");
                //使用前提是Trigger名必须和状态名相同
            }

            public override void OnEnter()
            {
                AnimDeltaTime = 0f;
                animLayer = 0;//默认情况
                if (animClipName.Contains("Lock") || animClipName.Contains("Target") || animClipName.Contains("Attack") || animClipName.Contains("PreSubAction")) animLayer = 1;

                Animator.Play($"{animClipName}", animLayer);//弊端!!!!!动画之间没有过度
               //Animator.SetTrigger($"{triggerName}");

                animInfo = Animator.GetCurrentAnimatorStateInfo(animLayer);//获取对应layer的动画State信息
                currentAnim = AnimTool.GetClipByName(Animator, animClipName);
                //Debug.Log($"{animClipName}动画已播放{animInfo.normalizedTime.ToString("p")}  {animInfo.IsName(currentAnim.name)}");
                //Debug.Log($"{currentAnim}{animClipName}Clip的动画时长为{currentAnim.length}秒,总帧数为{currentAnimFrame}");
            }
            public override void OnExit()
            {
                //Animator.ResetTrigger($"{triggerName}");
            }

        }
        //Update通用部分复用尝试
        //public override void OnUpdate(float deltaTime)
        //{
        //    if()有没有办法，唯独不识别Change<自己类型>的if 的方法？
        //    if (Role.InvincibleTime == 0 && Role.isAttacked) ChangeState<DamagedState>();
        //    if (Role.IsJumpPressed && Role.GroundDetect.IsGrounded) ChangeState<JumpState>();
        //    if (Role.canMoveHorizontal && Mathf.Abs(InputAxis.x) > 0.1f) ChangeState<MoveState>();
        //    if (Role.IsLockPressed) ChangeState<LockState>();
        //}
        // Role.animator.ResetTrigger("") 经过和State类名配合设计也可以达到复用效果
        // string TriggerName = this.name subString到State为止
    }
}