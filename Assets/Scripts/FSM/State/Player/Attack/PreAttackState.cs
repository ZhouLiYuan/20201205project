
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;



public class PreAttackState : PlayerRoleState
{
    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnUpdate(float deltaTime)
    {
        ////进行判断指定的动画是否播放完成   animInfo.IsName(animClipName)只适合在动画有连线时用（也就是有动画过渡IsInTransition(LayerIndex)）
        //var animInfo = Role.animator.GetCurrentAnimatorStateInfo(0);//Damaged动画所在层
        //if ( (animInfo.IsName("Damaged")) && (animInfo.normalizedTime < 1.0f)) return;//0 – 1, 0开始，1结束（如果当前还在播Damaged动画，就无法进行攻击）


        if (Role.IsAttackPressed)
        {
            switch (Role.currentWeapon.AssetID)
            {
                case 1:
                    ChangeState<SwordAttackState>();
                    return;
                case 2:
                    ChangeState<PunchAttackState>();
                    return;
                case 3:
                    ChangeState<GunAttackState>();
                    break;
                default:
                    break;
            }
        }
    }

    public override void OnExit()
    {

    }
}
