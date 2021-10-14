
public enum InteractableType {None,NPC, Item}
//这里面的类也许都会  ！！！！！！作废！！！！！！！！
[System.Serializable]
public class InteractableData
{
  public InteractableType interactableType;
}

[System.Serializable]
public class InteractableItemData : InteractableData
{
    public string itemName;
    public int itemId;
    public InteractableItemData() { interactableType = InteractableType.Item; }
}


[System.Serializable]
public class InteractableDialogueData : InteractableData
{
    public string roleName;
    public int episodeID;

    public InteractableDialogueData() { interactableType = InteractableType.NPC; }
}