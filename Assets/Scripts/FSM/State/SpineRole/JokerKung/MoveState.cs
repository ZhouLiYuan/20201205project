using UnityEngine;
using Spine;
using Spine.Unity;

namespace Role.SpineRole
{
 
    public class MoveState : SpineRoleState
    {

        //自己实现的回调（FSM中调用的函数）
        public override void OnEnter()
        {
            base.OnEnter();
            SetAnimation(Role.move01);
            Debug.Log($"Spine动画{Role.move01}");
        }
        public override void OnUpdate(float deltaTime)
        {
            //横向输入 x轴方向
            Velocity = new Vector2(InputAxis.x * MoveSpeed, Velocity.y);
            Role.TurnFace();
            //垂直方向 跳跃速度
            if (Role.IsJumpPressed && Role.GroundDetect.IsGrounded) ChangeState<JumpState>();
            //if (Role.InvincibleTime == 0 && Role.isAttacked) ChangeState<DamagedState>();
            if (InputAxis == Vector2.zero) ChangeState<IdleState>();
            //if (Velocity == Vector2.zero) ChangeState<IdleState>();
        }
        public override void OnExit() 
        {
            base.OnExit();
        }



        //Spine事件回调
        protected override void State_Start(TrackEntry trackEntry)//OnStateEnter
        {
            base.State_Start(trackEntry);
        }

        protected override void State_Complete(TrackEntry trackEntry)//On one duration finsh
        {
        }

        protected override void State_End(TrackEntry trackEntry)//OnStateExit
        {
            base.State_End(trackEntry);
            //if (Role.InvincibleTime == 0 && Role.isAttacked) ChangeState<DamagedState>();
        }


    }
}
