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


    public override void OnOpen() { }

    public virtual void SetOwner(GameObject owner) {   this.owner = owner;  }

    public override void OnClose() 
    {
        state = InteractState.Finish;
    }
}
