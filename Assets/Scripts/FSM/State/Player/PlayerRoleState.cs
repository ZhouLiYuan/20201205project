using UnityEngine;

/// <summary>
/// 角色状态 
/// 由于FSM分层，修改某state实例的字段无法同步到其他同名实例State
/// 所以字段的set应统一到PlayerRole
/// </summary>
public class PlayerRoleState:State
{
   public PlayerRole Role { get; private set; }
    protected GameObject role_Gobj => Role.GameObject;

    //一些输入
    protected Vector2 InputAxis => Role.inputAxis;
    protected float MoveSpeed => Role.moveSpeed;
    protected float JumpSpeed => Role.jumpSpeed;

    protected string triggerName;

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
        //在每个状态出入的时候可以  Animator.SetTrigger($"{triggerName}");
        //使用前提是Trigger名必须和状态名相同
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
