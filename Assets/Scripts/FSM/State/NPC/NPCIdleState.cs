using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCIdleState :NPCState<NPC>
{
    public override void OnEnter() 
    {
      NPCRole.animator.SetTrigger("Idle");
    }
    public override void OnUpdate(float deltaTime)
    {
        if(NPCRole.isInteractingWithPlayer) ChangeState<NPCInteractState>();
    }
    public override void OnExit()
    {
        NPCRole.animator.ResetTrigger("Idle");
    }
}
