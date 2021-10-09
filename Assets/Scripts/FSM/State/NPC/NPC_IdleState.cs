using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_IdleState :NPCState<NPC>
{
    public override void OnEnter() 
    {
      Role.animator.SetTrigger("Idle");
    }
    public override void OnUpdate(float deltaTime)
    {
        if(Role.isInteractingWithPlayer) ChangeState<NPC_InteractState>();
    }
    public override void OnExit()
    {
        Role.animator.ResetTrigger("Idle");
    }
}
