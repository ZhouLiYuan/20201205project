using UnityEngine;

public class JumpState : PlayerRoleState
{
    private int jumpCount;//剩余跳跃次数

    public override void OnEnter() 
    {
        //Role.animator.SetTrigger("Jump");
        base.OnEnter();
        //垂直方向 跳跃速度
        Velocity = new Vector2(Velocity.x, JumpSpeed);
        jumpCount = 1;
    }

    public override void OnUpdate(float deltaTime)
    {
        //滞空时也可以缓慢移动
        Velocity = new Vector2(InputAxis.x * MoveSpeed/2, Velocity.y);
        if (Role.GroundDetect.IsGrounded) ChangeState<IdleState>();
        if (jumpCount > 0 && Role.playerInput.Jump.triggered )//二段跳
        {
            Velocity = new Vector2(Velocity.x, JumpSpeed);
            jumpCount--;
        } 
        if (Role.isAttacked && Role.InvincibleTime <= 0) ChangeState<DamagedState>();
    }

    public override void OnExit()
    {
         //Role.animator.ResetTrigger("Jump");
    }
}
