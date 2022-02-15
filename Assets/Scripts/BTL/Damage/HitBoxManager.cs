using Role.SpineRole;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




//需要根据不同的motion动态生成相应的HitBox和HurtBox，应该在角色不同的State中，根据骨骼位置，在对应的时机生成
//或者用animation的方式实现
//需要一个SkillManager
public class HitBoxManager
{
    //字典，多colider对一playerRole
    public static Dictionary<Collider2D, PlayerRole_Spine> boxesDic = new Dictionary<Collider2D, PlayerRole_Spine>();
    //BtlCharacterManager

    public static void AddBoxsToDic(IEnumerable<Collider2D> boxes, PlayerRole_Spine owner)
    {
        foreach (var box in boxes) {boxesDic[box] = owner;}
    }
}
