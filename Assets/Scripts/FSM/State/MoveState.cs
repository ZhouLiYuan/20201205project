
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;

public class MoveState : PlayerRoleState
{
    public override void OnEnter()
    {
        Role.animator.SetTrigger("Move");
    }

    public override void OnUpdate(float deltaTime)
    {
        //包装了好几层，这里才是真正修改 Role刚体的速度的地方

        //横向输入 x轴方向
        Velocity = new Vector2(InputAxis.x * moveSpeed, Velocity.y);
        if (Role.InvincibleTime == 0 && Role.isAttacked) ChangeState<DamagedState>();
        //垂直方向 跳跃速度
        if (Role.IsJumpPressed && Role.GroundDetect.IsGrounded) ChangeState<JumpState>(); 
    }

    public override void OnExit()
    {

        Role.animator.ResetTrigger("Move");
    }
}
