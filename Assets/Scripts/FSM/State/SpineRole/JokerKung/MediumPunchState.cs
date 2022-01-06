using UnityEngine;
using Spine;
using Spine.Unity;

namespace Role.SpineRole
{
 
    public class MediumPunchState : SpineRoleState
    {

        //自己实现的回调（FSM中调用的函数）
        public override void OnEnter()
        {
            base.OnEnter();
            SetAnimation(0, "Attack01", false, 0, 0.75f);
            Role.canMoveHorizontal = false;
        }
        public override void OnUpdate(float deltaTime)
        {
            //if (Role.InvincibleTime == 0 && Role.isAttacked) ChangeState<DamagedState>();
        }
        public override void OnExit()
        {
            base.OnExit();
            Role.canMoveHorizontal = true;
        }


        //Spine事件回调
        protected override void State_Start(TrackEntry trackEntry)//OnStateEnter
        {
            base.State_Start(trackEntry);
        }

        protected override void State_Complete(TrackEntry trackEntry)//On one duration finsh
        {
            ChangeState<PreSubActionState>();
        }

        protected override void State_End(TrackEntry trackEntry)//OnStateExit
        {
            base.State_End(trackEntry);
            ChangeState<PreSubActionState>();
        }


    }
}
