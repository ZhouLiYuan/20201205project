using UnityEngine;
using UnityEngine.UI;

public class NPCInteractablePanel : InteractablePanel
{
    public override string Path => "Panel/NPCInteractablePanel.prefab";
    private Text dialogueText;
    private int EpisodeId =>StoryManager.currentEpisodeID;
    private string NPCName => StoryManager.InteractingNPCName;

    public enum NPCState { None,Talk,Trade,Quest}
    public NPCState NPCstate = NPCState.None;

    protected NPC npc;

    public override void OnOpen()
    {
        base.OnOpen();
        //dialogueText = Find<Text>("Text");
    }

    public override void SetOwner(GameObject owner)
    {
        base.SetOwner(owner);
        npc = NPCManager.nameDic[owner.name];
        dialogueText.text = $"与{npc.name}进行对话";

        //研究下对话内容咋整
        //按下R键会触发GlobalEvent.Interact
        PlayerManager.m_Role.playerInput.Interact.performed += GlobalEvent.Interact;
        //GlobalEvent.Interact会调用 GlobalEvent.OnPressInteract 
        GlobalEvent.OnPressInteract += this.OnPressInteract;
    }

    //在玩家进入Panel交互状态的情况下 再次按下交互键R  全局OnPressInteract被调用，OnPressInteract再调用 GlobalEvent.ShowDialogue(storyId);
    protected override void OnPressInteract(UnityEngine.InputSystem.InputAction.CallbackContext callbackContext)
    {
        GlobalEvent.ShowDialogue(EpisodeId,NPCName);
        //Close();
    }

    public override void OnClose()
    {
        PlayerManager.m_Role.playerInput.Interact.performed -= GlobalEvent.Interact;
        GlobalEvent.OnPressInteract -= OnPressInteract;
    }
}
