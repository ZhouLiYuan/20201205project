public class InteractableManager
{
    private InteractablePanel interactablePanel;

    public void Init()
    {
        GlobalEvent.OnEnterInteractArea += ShowInteractableUI;
        GlobalEvent.OnExitInteractArea += HideInteractableUI;

        interactablePanel = UIManager.Open<InteractablePanel>();
        interactablePanel.Hide();
    }

    private void ShowInteractableUI(InteractableData interactableData)
    {
        interactablePanel.Init(interactableData);
        interactablePanel.Show();
    }

    private void HideInteractableUI(InteractableData interactableData)
    {
        interactablePanel.Hide();
    }
}
