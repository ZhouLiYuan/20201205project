
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;



public class PunchAttackState : PlayerRoleState
{
    public override void OnEnter()
    {
        base.OnEnter();
        Role.currentWeapon.collider2D.enabled = true;
        Role.canMoveHorizontal = false;
    }

    public override void OnUpdate(float deltaTime)
    {
        AnimDeltaTime += deltaTime;
        if (AnimDeltaTime >= currentAnim.length) ChangeState<PreSubActionState>(); //动画播放完毕切回准备攻击状态（AnimDeltaTime比动画长度长就确保播完了）
    }

    public override void OnExit()
    {
        Role.canMoveHorizontal = true;
        Role.currentWeapon.collider2D.enabled = false;
    }
}
