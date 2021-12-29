using UnityEngine;
using Spine;
using Spine.Unity;

namespace Role.SpineRole
{
 
    public class JumpState : SpineRoleState
    {
        //自己实现的回调（FSM中调用的函数）
        public override void OnEnter()
        {
            base.OnEnter();
        }
        public override void OnUpdate(float deltaTime) 
        {
            //滞空时也可以缓慢移动
            Velocity = new Vector2(InputAxis.x * MoveSpeed / 2, Velocity.y);
            if (Role.GroundDetect.IsGrounded) ChangeState<IdleState>();
            //if (jumpCount > 0 && Role.playerInput.Jump.triggered)//二段跳
            //{
            //    Velocity = new Vector2(Velocity.x, JumpSpeed);
            //    jumpCount--;
            //}
            //if (Role.isAttacked && Role.InvincibleTime <= 0) ChangeState<DamagedState>();
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
