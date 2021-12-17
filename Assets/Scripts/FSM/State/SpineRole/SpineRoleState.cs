using UnityEngine;
using Spine;
using Spine.Unity;


namespace Role.SpineRole
{
    public class SpineRoleState : State
    {
        public SpineRole Role { get; private set; }
        protected GameObject role_Gobj => Role.GameObject;

        public void SetRole(SpineRole role)
        {
            Role = role;
            //Init();
        }

        #region 事件对应回调方法
        protected virtual void State_Start(TrackEntry trackEntry)//OnStateEnter
        {
        }

        protected virtual void State_Complete(TrackEntry trackEntry)//On one duration finsh
        {
        }

        protected virtual void State_End(TrackEntry trackEntry)//OnStateExit
        { 
        }
        #endregion
    }
}