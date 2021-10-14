
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;

public class MoveState : PlayerRoleState
{
    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnUpdate(float deltaTime)
    {
  
        //横向输入 x轴方向
        Velocity = new Vector2(InputAxis.x * MoveSpeed, Velocity.y);
        //垂直方向 跳跃速度
        if (Role.IsJumpPressed && Role.GroundDetect.IsGrounded) ChangeState<JumpState>();
        if (Role.InvincibleTime == 0 && Role.isAttacked) ChangeState<DamagedState>();
        if (Role.IsInteractPressed && Role.isInInteractArea)
        {
            Velocity = Vector2.zero;//防止角色滑出交互区域
            ChangeState<InteractState>();
        } 
      
        if (Velocity == Vector2.zero) ChangeState<IdleState>();
    }

    public override void OnExit()
    {
        //Role.animator.ResetTrigger("Move");
    }
}
