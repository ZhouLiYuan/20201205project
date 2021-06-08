using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : PlayerRoleState
{
    public override void OnUpdate(float deltaTime)
    {
        if (Role.IsLockPressed) ChangeState<LockState>();
    }
}
