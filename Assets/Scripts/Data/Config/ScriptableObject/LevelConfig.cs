using Role.NPCs;
using System.Collections.Generic;
using UnityEngine;
using System;

//LevelConfig成员的赋值在LevelEditorWindow 或 ParamInspector 中完成


public class LevelConfig : ScriptableObject
{
    //值类型
    public int levelID;
    public string levelName;

    //引用类型
    public List<EnemyInfo> EnemyInfos = new List<EnemyInfo>();
    public List<PlatformInfo> PlatformInfos = new List<PlatformInfo>();
    public List<NPCInfo> NPCInfos = new List<NPCInfo>();
    public List<CheckPointInfo> CheckPointInfos = new List<CheckPointInfo>();
}
