using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 和钩锁相关的状态
/// </summary>
public class MoveToTargetState : HookBaseState
{
    private float hookSpeed;
    private float FinalJumpSpeed => Role.finalJumpSpeed;

    //禁用重力 和 输入对左右移动的控制
    public override void OnEnter()
    {
        base.OnEnter();
       
        Role.canApplyGravity = false;
        Role.canMoveHorizontal = false;
        hookSpeed = Role.hookSpeed;
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
        HookGobj.transform.position = TargetPos;
        AnimDeltaTime += deltaTime;
        if (AnimDeltaTime > 1f) {ChangeState<PreSubActionState>();}//致空飞行时长不能超过1s

        hookSpeed = Mathf.Lerp(hookSpeed, hookSpeed * 0.75f, deltaTime);//每次减速
        Vector2 direction = TargetPos - role_Gobj.transform.position;
        if (direction.sqrMagnitude > MinDistance * MinDistance)  {Velocity = direction.normalized * hookSpeed; }
        else
        {
            Velocity = new Vector2(Velocity.x*0.2f, FinalJumpSpeed);
            ChangeState<PreSubActionState>();
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        Role.canApplyGravity = true;
        Role.canMoveHorizontal = true;
    }

}
