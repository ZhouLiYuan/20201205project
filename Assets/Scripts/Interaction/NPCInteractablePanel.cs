using UnityEngine;
using UnityEngine.UI;

public class NPCInteractablePanel : InteractablePanel
{
    public override string Path => "Panel/NPCInteractablePanel.prefab";
    private Text dialogueText;
    private int storyId;



    public override void OnOpen()
    {
        base.OnOpen();
        dialogueText = Find<Text>("Text");
    }

    public void Init(InteractableData interactableData)
    {
        
         switch (interactableData)
        {
            case InteractableDialogueData interactableDialogueData:
                dialogueText.text = $"与{interactableDialogueData.RoleName}进行对话";
                storyId = interactableDialogueData.StoryId;
                break;
        }
    }

    protected override void OnPressInteract(UnityEngine.InputSystem.InputAction.CallbackContext callbackContext)
    {
        GlobalEvent.ShowDialogue(storyId);
        Close();
    }
}
