public class InteractableManager
{
    private InteractablePanel m_interactablePanel;

    public void Init() 
    {
        GlobalEvent.OnEnterInteractArea += ShowInteractableUI;
        GlobalEvent.OnExitInteractArea += HideInteractableUI;
    }

    private void ShowInteractableUI(InteractableData data) 
    {
        m_interactablePanel.Init(data);
        m_interactablePanel.Show();
    }

    //虽然没有用到传入的参数，但这里只是为了保持和委托的参数列表一直（否在无法完成订阅）
    private void HideInteractableUI(InteractableData data) { m_interactablePanel.Hide(); }
}
