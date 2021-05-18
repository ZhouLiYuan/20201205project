using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 关卡信息
/// </summary>
public class LevelData : ScriptableObject
{
    /// <summary>
    /// 场景序号
    /// </summary>
    public int levelIndex;
    /// <summary>
    /// 关卡中的敌人列表
    /// </summary>
    public List<EnemyInfo> EnemyInfos = new List<EnemyInfo>();
}
