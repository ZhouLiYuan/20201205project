using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class InteractablePanel : BasePanel
{
    public enum InteractState { None,Interact,Finish }
    public InteractState state = InteractState.None;


    //面板持有者
    public GameObject owner;




    //根据角色不同的交互状态显示不同UI 比如 任务-惊叹号 普通状态-向下箭头
    public string hintUIName = "Arrow";

    public override void OnOpen()
    {
        //interactableHint = Find<GameObject>("InteractableHint");
        //guider = interactableHint.GetComponent<GameObject>().sprite;
        //GlobalEvent.OnPressInteract += OnPressInteract;
    }

    public virtual void SetOwner(GameObject owner)
    {
        this.owner = owner;
    }



    protected virtual void OnInteract(UnityEngine.InputSystem.InputAction.CallbackContext callbackContext)
    {
       
    }
}
