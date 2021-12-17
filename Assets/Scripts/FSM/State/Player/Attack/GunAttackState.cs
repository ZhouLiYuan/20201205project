
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;


namespace Role.SelectableRole
{
        public class GunAttackState : PlayerRoleState
        {
            public override void OnEnter()
            {
                base.OnEnter();
                Role.canMoveHorizontal = false;//不能边攻击边移动
                Role.currentWeapon.collider2D.enabled = true;//进入攻击状态时武器才能与敌人产生碰撞
                Role.animEvent.AttackEvent += Role.Shoot;
            }

            public override void OnUpdate(float deltaTime)
            {
                AnimDeltaTime += deltaTime;

                //Debug.Log($"{animClipName}动画已播放{AnimDeltaTime}秒 ");
                if (AnimDeltaTime >= currentAnim.length) ChangeState<PreSubActionState>(); //动画播放完毕切回准备攻击状态（AnimDeltaTime比动画长度长就确保播完了）
            }

            public override void OnExit()
            {
                Role.canMoveHorizontal = true;
                Role.currentWeapon.collider2D.enabled = false;
                Role.animEvent.AttackEvent -= Role.Shoot;
            }
        }
}

//暂时没搞懂原理的API animInfo.normalizedTime >= 1f
//public override void OnUpdate(float deltaTime)
//{
//    //横向输入 x轴方向
//    Velocity = new Vector2(InputAxis.x * MoveSpeed, Velocity.y);
//    //垂直方向 跳跃速度
//    if (animInfo.normalizedTime >= 1f) /*&& currentAnim.name == animClipName (弃用(animInfo.IsName(animClipName)))*/
//    {
//        Debug.Log($"{animClipName}动画已播放{animInfo.normalizedTime.ToString("p")} ");
//        ChangeState<PreAttackState>(); //动画播放完毕切会准备攻击状态
//    }
//}
