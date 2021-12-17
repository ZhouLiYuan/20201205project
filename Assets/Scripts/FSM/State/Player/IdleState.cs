using UnityEngine;

namespace Role.SelectableRole
{
        public class IdleState : PlayerRoleState
        {
            public override void OnEnter()
            {
                //Role.animator.SetTrigger("Idle");
                base.OnEnter();
            }
            public override void OnUpdate(float deltaTime)
            {
                if (Role.InvincibleTime == 0 && Role.isAttacked) ChangeState<DamagedState>();
                if (Role.IsJumpPressed && Role.GroundDetect.IsGrounded) ChangeState<JumpState>();
                if (Role.canMoveHorizontal && Mathf.Abs(InputAxis.x) > 0.1f) ChangeState<MoveState>();
                if (Role.IsInteractPressed && Role.isInInteractArea) ChangeState<InteractState>();
            }
            public override void OnExit()
            {
                //Role.animator.ResetTrigger("Idle");
            }
        }
}
