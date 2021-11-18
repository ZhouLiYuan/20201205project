using System;
using UnityEngine;

public class InfoCollections
{

}



public class EntityInfo
{
    public Vector3 Position;
    public int AssetID;
}


[Serializable]
public class EnemyInfo : EntityInfo
{
    public int WeaponID;
}

[Serializable]
public class NPCInfo : EntityInfo
{
}

[Serializable]
public class PlatformInfo
{
    public string AssetName;
}

[Serializable]
public class CheckPointInfo: ScriptableObject
{

}
