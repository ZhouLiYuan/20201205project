using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Role.SelectableRole
{
        public class InteractState : PlayerRoleState
        {
            public override void OnEnter()
            {
                Debug.Log("玩家进入交互状态");
                base.OnEnter();
                Role.Interact();

            }

            public override void OnUpdate(float deltaTime)
            {
                if (Role.currentInteractingNPC.interactingPanel == null) return;//防报错：player的交互OnUpdate()执行时机比NPC OnEnter()还早一帧
                if (Role.currentInteractingNPC.interactingPanel.state == InteractablePanel.InteractState.Finish)
                {
                    ChangeState<IdleState>();
                }

            }

            public override void OnExit()
            {
                Role.ExitInteract();
            }
        }
}