using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteractablePanel : InteractablePanel
{
    public override string Path => "Panel/ItemInteractablePanel.prefab";

    public enum ItemState { DoNothing,Get,Abandon}
    public ItemState itemState = ItemState.DoNothing;

    public override void OnOpen()
    {
        base.OnOpen();
    }

}
