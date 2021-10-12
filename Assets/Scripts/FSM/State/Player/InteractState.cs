using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractState : PlayerRoleState
{
    public override void OnEnter()
    {
        Debug.Log("玩家进入交互状态");
        Animator.SetTrigger($"{triggerName}");
        Role.Interact();
        //禁用交互键以外的所有按键
        Role.playerInput.DisableInput();
        Role.playerInput.Interact.Enable();
    }

    public override void OnUpdate(float deltaTime)
    {
        Role.IsInteracting = true;
        //if (Role.currentInteractingNPC.interactingPanel.state == InteractablePanel.InteractState.Finish) ChangeState<IdleState>();
    }

    public override void OnExit()
    {
        Animator.ResetTrigger($"{triggerName}");
        Role.IsInteracting = false;
        //恢复所有按键
        Role.playerInput.EnableInput();
    }
}
