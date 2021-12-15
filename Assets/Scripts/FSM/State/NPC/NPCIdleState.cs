namespace Role
{
        namespace NPCs
        {
            public class NPCIdleState : NPCState<NPC>
            {
                public override void OnEnter()
                {
                    base.OnEnter();
                }
                public override void OnUpdate(float deltaTime)
                {
                    if (PlayerManager.p1_Role.nearestInteractableGobj == NPCRole.GameObject) ChangeState<NPCPreInteractState>();
                }
                public override void OnExit()
                {
                }
            }
        }
    }
