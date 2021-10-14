using System;
using UnityEngine;
//解决难以获得实例 以及 传参难问题
//适合 发生时机相同 关联度高 有执行先后 适合集中调用，在同一帧处理的

//像和很多实例相关且发生时机不同，不适合集中调用的不适合放在这里
//（比如敌人 受击特效 就是在各个敌人不同的受击瞬间分别调用，关联不大）
public static class GlobalEvent
{
    public static event Action<int,string> OnShowDialogue;
    public static event Action<UnityEngine.InputSystem.InputAction.CallbackContext> OnPressInteract;

    public static void ShowDialogue(int storyId,string npcName) { OnShowDialogue?.Invoke(storyId,npcName); }
    public static void Interact(UnityEngine.InputSystem.InputAction.CallbackContext obj)  {OnPressInteract?.Invoke(obj);}
}
