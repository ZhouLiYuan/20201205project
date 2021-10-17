using System;
using UnityEngine;

//像和很多实例相关且发生时机不同，不适合集中调用的不适合放在这里
//（比如敌人 受击特效 就是在各个敌人不同的受击瞬间分别调用，关联不大）
public static class GlobalEvent
{
    ////耦合到DialogueSystem（由于不够面想对象，取消） 
    //public static event Action<int> OnShowDialogue;
    //public static void ShowDialogue(int storyId) { OnShowDialogue?.Invoke(storyId); }//交互NPC面板对话按钮触发交互NPC面板对话按钮触发



    //PlayerManager.m_Role.playerInput.Interact.performed本身就够全局了，不用下面代码（如果是多人操作(难访问到各个Player实例)游戏才要）
    //public static event Action<UnityEngine.InputSystem.InputAction.CallbackContext> OnPressInteract;
    //public static void Interact(UnityEngine.InputSystem.InputAction.CallbackContext obj)  {OnPressInteract?.Invoke(obj); }//按下R键触发
}
