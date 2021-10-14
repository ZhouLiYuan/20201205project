using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//LevelConfig成员的赋值在LevelEditorWindow中完成

/// <summary>
/// 关卡信息
/// 目前只有 LevelConfig 信息来自于ScriptableObject
/// </summary>
public class LevelConfig : ScriptableObject
{
   
    //值类型
    public int levelID;
    public string levelName;

    //引用类型
    public List<EnemyInfo> EnemyInfos = new List<EnemyInfo>();
    public List<PlatformInfo> PlatformInfos = new List<PlatformInfo>();
    public List<NPC> NPCs = new List<NPC>();
    public List<CheckPointInfo> CheckPointInfos = new List<CheckPointInfo>();

}
