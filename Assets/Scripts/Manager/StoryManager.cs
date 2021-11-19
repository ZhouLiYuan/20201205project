using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryManager
{
    public static int currentStoryID;
    public static int currentChapterID;
    public static int currentEpisodeID = 0;

    public static string currentPlaceName =>PlayerManager.m_Role.currentPlaceName;
    public static string InteractingNPCName;
 

    //非写死剧情，可以由玩家条件与对话时机解锁？
    //3key对1value，key按顺序: currentPlaceName，currentNPCRoleName，currentEpisodeID
    public static Dictionary<string, Dictionary<string, Dictionary<int, DialogueConfig[]>>> conditionDialogueDic = 
        new Dictionary<string, Dictionary<string, Dictionary<int, DialogueConfig[]>>>();
}
