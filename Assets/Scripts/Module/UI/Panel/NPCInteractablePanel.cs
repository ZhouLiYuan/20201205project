using Role;
using Role.NPCs;
using UnityEngine;
using UnityEngine.UI;

public class NPCInteractablePanel : InteractablePanel
{
    private string NPCName => StoryManager.InteractingNPCName;


    protected NPC npc;
  

    public override void SetOwner(GameObject owner)
    {
        base.SetOwner(owner);
        npc = NPCManager.nameDic[owner.name];
    }

   

    public override void OnOpen()
    {
        base.OnOpen();
        state = InteractState.Interact;
       
        Find<Button>("ConversationButton").onClick.AddListener(() => OnConversation());
        Find<Button>("QuestButton").onClick.AddListener(() => Debug.Log("任务"));//任务
        Find<Button>("TradeButton").onClick.AddListener(() => Debug.Log("交易"));//交易
        Find<Button>("QuitButton").onClick.AddListener(() => {
            OnClose();
            Close();
        });
    }


    private void OnConversation()
    {
        npc.interactState = NPC.InteractState.Talk;
        Hide();
        var temp = UIManager.Open<DialoguePanel>();
        temp.temp = this;
    }

    public override void OnClose()
    {
        base.OnClose();
        state = InteractState.Finish;
    }


}
