using System;

public static class GlobalEvent
{
    public static event Action<InteractableData> OnEnterInteractArea;
    public static event Action<InteractableData> OnExitInteractArea;
    public static event Action<int> OnShowDialogue;

    public static event Action<UnityEngine.InputSystem.InputAction.CallbackContext> OnPressInteract;

  
    public static event Action<DamageData> OnDamaged;


    public static void EnterInteractArea(InteractableData data) { OnEnterInteractArea?.Invoke(data); }
    public static void ExitInteractArea(InteractableData data) { OnExitInteractArea?.Invoke(data); }
    public static void ShowDialogue(int storyId) { OnShowDialogue?.Invoke(storyId); }

    public static void Interact(UnityEngine.InputSystem.InputAction.CallbackContext obj)  {OnPressInteract?.Invoke(obj);}

    public static void Damage(DamageData data) { OnDamaged?.Invoke(data); }
}
