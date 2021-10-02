using UnityEngine;
using System;

public class InteractableController
{
    //玩家正在交互的对象
    public  GameObject InteractingObj;

    //玩家正在交互的面板类型
    public InteractablePanel interactablePanel;

    public  event Action<InteractableData> OnEnterInteractArea;
    public void EnterInteractArea(InteractableData data) { OnEnterInteractArea?.Invoke(data); }
    public event Action<InteractableData> OnExitInteractArea;
    public  void ExitInteractArea(InteractableData data) { OnExitInteractArea?.Invoke(data); }



    public void Init<TInteractablePanel>() where TInteractablePanel : InteractablePanel, new()
    {
        OnEnterInteractArea += ShowInteractableUI;
        //m_role.OnInteract += InteractableManager.interactablePanel.SetHintUI;
        OnExitInteractArea += HideInteractableUI;

        interactablePanel = UIManager.Open<TInteractablePanel>();
        interactablePanel.Hide();
    }

    private  void ShowInteractableUI(InteractableData interactableData)
    {
        //interactablePanel.Init(interactableData);
        //interactablePanel.Show();
    }

    private  void HideInteractableUI(InteractableData interactableData)
    {
        interactablePanel.Hide();
    }
}
