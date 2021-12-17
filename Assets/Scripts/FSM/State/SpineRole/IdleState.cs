using UnityEngine;
using Spine;
using Spine.Unity;

namespace Role.SpineRole
{
    public class IdleState: SpineRoleState
    {
        //Spine回调
        protected override void State_Start(TrackEntry trackEntry)//OnStateEnter
        {
        }

        protected override void State_Complete(TrackEntry trackEntry)//On one duration finsh
        {
        }

        protected override void State_End(TrackEntry trackEntry)//OnStateExit
        {
        }

        //自己实现的回调（FSM中调用的函数）
        public override void OnEnter() { }
        public override void OnUpdate(float deltaTime) { }
        public override void OnExit() { }
    }
}