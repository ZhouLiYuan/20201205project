using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedState : PlayerRoleState
{
    public override void OnEnter()
    {
        Role.InvincibleTime = Role.invincibleInterval;
        base.OnEnter();

        //特效生成  以后可以根据武器 攻击类型 或者角色 盔甲类型生成不同种类（倾向于前者）
        //var hitEffectPrefab = ResourcesLoader.LoadEffectPrefab("ef_hit01");
        //var hitEffectGobj = Object.Instantiate(hitEffectPrefab, Role.Transform.position, Quaternion.identity);
        Role.GetDamage();
    }

    public override void OnUpdate(float deltaTime)
    {
        AnimDeltaTime += deltaTime;
        Role.InvincibleTime -= deltaTime;
        Debug.Log($"剩余无敌时间{Role.InvincibleTime}");
        //if (Role.canMoveHorizontal && Mathf.Abs(InputAxis.x) > 0.1f) Velocity = new Vector2(InputAxis.x * moveSpeed, Velocity.y);

        //进行判断当前动画是否播放完成    
        //if ((animInfo.normalizedTime >= 1.0f))//normalizedTime: 范围0 – 1, 0是动作开始，1是动作结束
        if (AnimDeltaTime >= currentAnim.length){Role.animator.Play("Idle", 0);}

        //攻击结束后重置冷却时间
        if (Role.InvincibleTime == 0) ChangeState<IdleState>();
    }

    public override void OnExit()
    {
        PlayerManager.m_Role.isAttacked = false;
        //Role.animator.ResetTrigger("Damaged");
    }
}
