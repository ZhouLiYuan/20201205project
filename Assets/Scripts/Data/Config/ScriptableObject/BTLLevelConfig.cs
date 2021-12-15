using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTLLevelConfig
{
    //值类型
    public int levelID;
    public string levelName;

    //引用类型
    public EnemyInfo EnemyInfo = new EnemyInfo();
    public List<BackGround> BackGroundInfos = new List<BackGround>();
}
