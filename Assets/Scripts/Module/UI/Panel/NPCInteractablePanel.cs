using UnityEngine;
using UnityEngine.UI;

public class NPCInteractablePanel : InteractablePanel
{
    private int EpisodeId =>StoryManager.currentEpisodeID;
    private string NPCName => StoryManager.InteractingNPCName;


    protected NPC npc;
  

    public override void SetOwner(GameObject owner)
    {
        base.SetOwner(owner);
        npc = NPCManager.nameDic[owner.name];

    }

   
    protected override void OnInteract(UnityEngine.InputSystem.InputAction.CallbackContext callbackContext)
    {
        GlobalEvent.ShowDialogue(EpisodeId,NPCName);
        //Close();
    }

    public override void OnOpen()
    {
        base.OnOpen();
        state = InteractState.Interact;
        //全局OnPressInteract被调用，OnPressInteract再调用 GlobalEvent.ShowDialogue(storyId);
        Find<Button>("ConversationButton").onClick.AddListener(() => OnConversation());
        Find<Button>("QuestButton").onClick.AddListener(() => Debug.Log("任务"));//任务
        Find<Button>("TradeButton").onClick.AddListener(() => Debug.Log("交易"));//交易
        Find<Button>("QuitButton").onClick.AddListener(() => {
            OnClose();
            Close();
        });

    }

    public override void OnClose()
    {
        state = InteractState.Finish;
        PlayerManager.m_Role.playerInput.Interact.performed -= GlobalEvent.Interact;
        GlobalEvent.OnPressInteract -= OnInteract;
        state = InteractState.Finish;
    }

    private void OnConversation() 
    {
        npc.interactState = NPC.InteractState.Talk;
        //按下R键会触发GlobalEvent.Interact
        PlayerManager.m_Role.playerInput.Interact.performed += GlobalEvent.Interact;
        //GlobalEvent.Interact会调用 GlobalEvent.OnPressInteract 
        GlobalEvent.OnPressInteract += this.OnInteract;
    }

}
