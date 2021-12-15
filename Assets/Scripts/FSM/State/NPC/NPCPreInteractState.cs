using UnityEngine;

namespace Role
{
    namespace NPCs
    {
        //准备交互
        public class NPCPreInteractState : NPCState<NPC>
        {
            GameObject guider;

            public override void OnEnter()
            {
                base.OnEnter();
                guider = UIManager.SpawnGuiderUI(UIManager.HintUIName, NPCRole.Transform);
            }

            public override void OnUpdate(float deltaTime)
            {
                var isNearest = (PlayerManager.p1_Role.nearestInteractableGobj == NPCRole.GameObject);
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
    }
}