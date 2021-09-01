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

    //这种偏逻辑的方法放在 面板脚本里会不会有些不太合适？
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
