using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractState : NPCState<NPC>
{
    public override void OnEnter()
    {
        Debug.Log("NPC进入交互状态");
        NPCRole.animator.SetTrigger("Interact");
        //NPC打开面板
        NPCRole.Interact(NPCRole.GameObject);
    }

    public override void OnUpdate(float deltaTime)
    {
        //结束交互时返回(比如在每个InteractPanel里都放一个对应的State或者写一个事件通知) 待机状态
        //if (NPCRole.interactingPanel.state == InteractablePanel.InteractState.Finish) ChangeState<NPCIdleState>();
    }


    public override void OnExit()
    {
        NPCRole.isInteractingWithPlayer = false;
        NPCRole.animator.ResetTrigger("Interact");
    }
}
