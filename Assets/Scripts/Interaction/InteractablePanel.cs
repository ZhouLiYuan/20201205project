using UnityEngine.UI;

public class InteractablePanel:BasePanel
{
    public override string Path => "Panel/InteractablePanel.prefab";

    private Text dialogueText;
    private int storyId;

    public override void OnOpen() 
    {
        dialogueText = Find<Text>("Text");
        GlobalEvent.OnPressInteract += OnPressInteract;
    }


    private void OnPressInteract(UnityEngine.InputSystem.InputAction.CallbackContext callbackContext) 
    {
        GlobalEvent.ShowDialogue(storyId);
        Close();
    }

   
    public void Init(InteractableData data)
    {
        switch (data)
        {
            case InteractableDialogueData interactableDialogueData:
                dialogueText.text = $"与{interactableDialogueData.RoleName}进行对话";
                storyId = interactableDialogueData.storyId;
                break;
        }
    }
}
