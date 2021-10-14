using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCIdleState :NPCState<NPC>
{
    public override void OnEnter() 
    {
        base.OnEnter();
    }
    public override void OnUpdate(float deltaTime)
    {
        if (PlayerManager.m_Role.nearestInteractableGobj == NPCRole.GameObject) ChangeState<NPCPreInteractState>();
    }
    public override void OnExit()
    {
    }
}
