using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


//需要用subString或者split /n之类的来获取敌人Attacker种类名称

/// <summary>
/// 为场景中的所有对应名称的敌人加装逻辑层脚本(使用前提，场景中的敌人全都生成完毕)
/// 和其他现成LoadAsset的资源不同 Attacker是敌人Gobj身体某个子物体，是现成的需要获得（控制层级一致）
/// </summary>
public static class WeaponManager
{
    private static List<GameObject> attackerGobjs = new List<GameObject>();
    private static List<BaseWeapon> attackers = new List<BaseWeapon>();

    private static int GobjIndex;

    private static Type StringToType(GameObject attacker)
    {
        string typeName = attacker.name.Substring(8);
        var AttackerType = Type.GetType(typeName);
        return AttackerType;
    }

    public static void AddAttackers() 
    {
        if (attackers.Count == 0) return;

        for (GobjIndex = 0;GobjIndex < attackers.Count; GobjIndex++)
        {
            var AttackeType = StringToType(attackerGobjs[GobjIndex]);

            //或者到这里采用一个switch语句的形式？
            var attackerInstance = AttackeType.GetMethod("InitAttackerScript").MakeGenericMethod(new Type[] {AttackeType});
            //attackers.Add(attackerInstance);
        }
    }



    public static TAttacker InitWeapon<TAttacker>() where TAttacker : BaseWeapon, new()
    {
        TAttacker attacker = new TAttacker();
        attacker.Init(attackerGobjs[GobjIndex]);
        return attacker;
    }
}

