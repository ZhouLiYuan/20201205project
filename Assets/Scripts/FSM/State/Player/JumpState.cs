using UnityEngine;

public class JumpState : PlayerRoleState
{
    public override void OnEnter() 
    {
        Role.animator.SetTrigger("Jump");
        //垂直方向 跳跃速度
        Velocity = new Vector2(Velocity.x, JumpSpeed);

    }

    public override void OnUpdate(float deltaTime)
    {
        //滞空时也可以缓慢移动
        Velocity = new Vector2(InputAxis.x * MoveSpeed/2, Velocity.y);
        if (Role.GroundDetect.IsGrounded) ChangeState<IdleState>();
        if (Role.isAttacked && Role.InvincibleTime <= 0) ChangeState<DamagedState>();
    }

    public override void OnExit()
    {
         Role.animator.ResetTrigger("Jump");
    }
}
