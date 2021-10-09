using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_InteractState : NPCState<NPC>
{

    public override void OnEnter()
    {
        Role.animator.SetTrigger("");
        //NPC打开面板
        Role.Interact();
    }

    public override void OnUpdate(float deltaTime)
    {

        //结束交互时返回(比如在每个InteractPanel里都放一个对应的State或者写一个事件通知) 待机状态
        if (Role.interactingPanel.state == InteractablePanel.InteractState.Finish) ChangeState<NPC_IdleState>();
    }


    public override void OnExit()
    {
        Role.isInteractingWithPlayer = false;
        Role.animator.ResetTrigger("");
    }
}
