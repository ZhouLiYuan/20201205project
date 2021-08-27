using System;

//一个事件对应一个（事件非空时的调用方法）
public static class GlobalEvent
{
    public static event Action<InteractableData> OnEnterInteractArea;
    public static event Action<InteractableData> OnExitInteractArea;
    public static event Action<int> OnShowDialogue;

    //这一步是在干嘛，汇总所有的按键事件通知吗？
    public static event Action<UnityEngine.InputSystem.InputAction.CallbackContext> OnPressInteract;

    public static void EnterInteractArea(InteractableData data) { OnEnterInteractArea?.Invoke(data); }
    public static void ExitInteractArea(InteractableData data) { OnExitInteractArea?.Invoke(data); }
    public static void ShowDialogue(int storyId) { OnShowDialogue?.Invoke(storyId); }

    public static void Interact(UnityEngine.InputSystem.InputAction.CallbackContext obj) { OnPressInteract?.Invoke(obj); }
}
