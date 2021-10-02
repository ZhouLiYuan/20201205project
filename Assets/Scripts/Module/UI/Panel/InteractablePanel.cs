using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablePanel : BasePanel
{
    //向下三角箭头
    protected GameObject interactableHint;
    protected Sprite hintSprite;

    //根据角色不同的交互状态显示不同UI 比如 任务-惊叹号 普通状态-向下箭头
    public virtual string hintUIName { get; }

    public override void OnOpen()
    {
        interactableHint = Find<GameObject>("InteractableHint");
        hintSprite = interactableHint.GetComponent<SpriteRenderer>().sprite;
        GlobalEvent.OnPressInteract += OnPressInteract;
    }

    public void Init(InteractableData interactableData) 
    {
    }

    public void SetHintUI(GameObject target)
    {
        hintSprite = AssetModule.LoadAsset<Sprite>($"UI/Sprites/{hintUIName}.png");

        UIManager.SetInteractUI(target, interactableHint);
    }

    protected virtual void OnPressInteract(UnityEngine.InputSystem.InputAction.CallbackContext callbackContext)
    {
       
    }
}
