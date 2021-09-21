using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablePanel : BasePanel
{
    //向下三角箭头
    protected GameObject interactableHint;
    protected Sprite hintSprite;
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

    protected void SetHintUI(string hintName)
    {
        hintSprite = AssetModule.LoadAsset<Sprite>($"UI/Sprites/{hintName}.png");
    }

    protected virtual void OnPressInteract(UnityEngine.InputSystem.InputAction.CallbackContext callbackContext)
    {
       
    }
}
