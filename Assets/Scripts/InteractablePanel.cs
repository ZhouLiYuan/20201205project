using UnityEngine.UI;

public class InteractablePanel : BasePanel
{
    public override string Path => "Panel/InteractablePanel.prefab";
    private Text dialogueText;
    private int storyId;

    public override void OnOpen()
    {
        dialogueText = Find<Text>("Text");
        GlobalEvent.OnPressInteract += OnPressInteract;
    }

    public void Init(InteractableData interactableData)
    {
        switch (interactableData)
        {
            case InteractableDialogueData interactableDialogueData:
                dialogueText.text = $"与进行{interactableDialogueData.RoleName}对话";
                storyId = interactableDialogueData.StoryId;
                break;
        }
    }

    private void OnPressInteract(UnityEngine.InputSystem.InputAction.CallbackContext callbackContext)
    {
        GlobalEvent.ShowDialogue(storyId);
        Close();
    }
}
