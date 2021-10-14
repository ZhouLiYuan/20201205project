using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//准备交互
public class NPCPreInteractState : NPCState<NPC>
{
    GameObject guider;

    public override void OnEnter()
    {
        base.OnEnter();
        guider = UIManager.SpawnGuiderUI("Arrow", NPCRole.Transform);
    }

    public override void OnUpdate(float deltaTime)
    {
        var isNearest = (PlayerManager.m_Role.nearestInteractableGobj == NPCRole.GameObject);
        NPCRole.SetOutlineActive(isNearest);
        NPCRole.LookAtPlayer();
        //if (!PlayerManager.m_Role.isInInteractArea) ChangeState<NPCIdleState>();
        if (!isNearest) ChangeState<NPCIdleState>();
        if (NPCRole.isInteractingWithPlayer) ChangeState<NPCInteractState>();
    }

    public override void OnExit()
    {
        UIManager.DestoryGuiderUI(guider);
    }
}
