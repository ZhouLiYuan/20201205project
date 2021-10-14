using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedState : PlayerRoleState
{
    public override void OnEnter()
    {
        Role.InvincibleTime = Role.invincibleInterval;
        //Role.animator.SetTrigger("Damaged");
        Role.animator.Play("Damaged", 1);

        //特效生成  以后可以根据武器 攻击类型 或者角色 盔甲类型生成不同种类（倾向于前者）
        //var hitEffectPrefab = ResourcesLoader.LoadEffectPrefab("ef_hit01");
        //var hitEffectGobj = Object.Instantiate(hitEffectPrefab, Role.Transform.position, Quaternion.identity);
        Role.GetDamage();
    }

    public override void OnUpdate(float deltaTime)
    {
        Role.InvincibleTime -= deltaTime;
        Debug.Log($"剩余无敌时间{Role.InvincibleTime}");
        //if (Role.canMoveHorizontal && Mathf.Abs(InputAxis.x) > 0.1f) Velocity = new Vector2(InputAxis.x * moveSpeed, Velocity.y);

        //获取动画所在的层
        AnimatorStateInfo animatorInfo = Role.animator.GetCurrentAnimatorStateInfo(0);
        //进行判断指定的动画是否播放完成 cube是指动画的名称
        if ((animatorInfo.normalizedTime >= 1.0f) && (animatorInfo.IsName("damage")))//normalizedTime: 范围0 – 1, 0是动作开始，1是动作结束
        {
            Role.animator.Play("idle", 0);
        }

        //攻击结束后重置冷却时间
        if (Role.InvincibleTime == 0) ChangeState<IdleState>();
    }

    public override void OnExit()
    {
        PlayerManager.m_Role.isAttacked = false;
        //Role.animator.ResetTrigger("Damaged");
    }
}
