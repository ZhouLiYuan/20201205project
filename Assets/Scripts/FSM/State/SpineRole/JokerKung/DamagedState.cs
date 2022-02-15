using UnityEngine;
using Spine;
using Spine.Unity;

namespace Role.SpineRole
{
 
    public class DamagedState : SpineRoleState
    {
        //自己实现的回调（FSM中调用的函数）
        public override void OnEnter()
        {
       
            base.OnEnter();

            //特效生成完全由地方攻击类型决定
            Role.GetDamage();
        }
        public override void OnUpdate(float deltaTime)
        {
           //被练多招才会倒地进入无敌状态
        }
        public override void OnExit() { }



        //Spine事件回调
        protected override void State_Start(TrackEntry trackEntry)//OnStateEnter
        {
            base.State_Start(trackEntry);
        }

        protected override void State_Complete(TrackEntry trackEntry)//On one duration finsh
        {
            ChangeState<IdleState>();
        }

        protected override void State_End(TrackEntry trackEntry)//OnStateExit
        {
            base.State_End(trackEntry);
            //if (Role.InvincibleTime == 0 && Role.isAttacked) ChangeState<DamagedState>();
        }


    }
}
