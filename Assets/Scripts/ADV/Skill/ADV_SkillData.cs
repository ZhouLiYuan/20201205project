using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADV_SkillData
{
    public enum SkillIndex
    {
        Skill_01,
        Skill_02,
        Skill_03,
        Skill_04
    }

    public enum TargetSide 
    {
        Friend = 0,
        Enemy = 1,
        All = 2
    }


    //一些有待参考的枚举
    public enum EventType
    {
        None,
        FX,
        Reaction,
        Popup
    }

    public enum ReactionType
    {
        None,
        Damage,
        Heal
    }

}

