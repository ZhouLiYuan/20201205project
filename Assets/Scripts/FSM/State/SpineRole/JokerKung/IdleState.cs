using UnityEngine;
using Spine;
using Spine.Unity;

namespace Role.SpineRole
{
 
    public class IdleState : SpineRoleState
    {

        //自己实现的回调（FSM中调用的函数）
        public override void OnEnter()
        {
            base.OnEnter();
            SetAnimation(Role.idle01);
        }
        public override void OnUpdate(float deltaTime)
        {
            //if (Role.InvincibleTime == 0 && Role.isAttacked) ChangeState<DamagedState>();
            if (Role.IsJumpPressed && Role.GroundDetect.IsGrounded) ChangeState<JumpState>();
            if (Role.canMoveHorizontal && Mathf.Abs(InputAxis.x) > 0.1f) ChangeState<MoveState>();
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
