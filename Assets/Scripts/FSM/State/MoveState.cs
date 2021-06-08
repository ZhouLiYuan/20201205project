using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : PlayerRoleState
{
    //需要用odin序列化
    private float jumpSpeed = 8f;
    private float moveSpeed = 5f;

    public override void OnUpdate(float deltaTime)
    {
        //包装了好几层，这里才是真正修改 Role刚体的速度的地方

        //横向输入 x轴方向
        if (Role.canMoveHorizontal && Mathf.Abs(InputAxis.x) > 0.1f) Velocity = new Vector2(InputAxis.x * moveSpeed, Velocity.y);

        //垂直方向 跳跃速度
        if (Role.IsJumpPressed && Role.GroundDetect.IsGrounded) Velocity = new Vector2(Velocity.x, jumpSpeed);
    }
}
