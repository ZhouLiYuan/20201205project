using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 和钩锁相关的状态
/// </summary>
public class MoveToTargetState : PlayerRoleState
{
    private GameObject m_target;

    public void SetTarget(GameObject target) {this.m_target = target; }



    //需要序列化的变量
    private float hookSpeed = 10f;
    //触发最后向上速度 的 距离平台距离
    private float minDistance = 2f;
    //最后便于着陆的上升速度
    private float finalJumpSpeed = 4f;

    //禁用重力 和 输入对左右移动的控制
    public override void OnEnter()
    {
        Role.canApplyGravity = false;
        Role.canMoveHorizontal = false;
    }

    public override void OnUpdate(float deltaTime)
    {
        Vector3 direction = m_target.transform.position - role_Gobj.transform.position;
        if (direction.sqrMagnitude > minDistance * minDistance)  {Velocity = direction.normalized * hookSpeed; }
        else
        {
            Velocity = new Vector2(Velocity.x, finalJumpSpeed);

            //最后指定下一个状态
            ChangeState<IdleState>();
        }
    }

    public override void OnExit()
    {
        Role.canApplyGravity = true;
        Role.canMoveHorizontal = true;
    }

}
