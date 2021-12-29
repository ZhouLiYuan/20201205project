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
        }
        public override void OnUpdate(float deltaTime) { }
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
