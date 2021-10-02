using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DamageSystem
{



    public static void Init() 
    {
        
    }


    /// <summary>
    /// 返回值是计算后得出的傷害值
    /// </summary>
    /// <returns></returns>
    public static int CalculateDamage(DamageData data) 
    {
        int damageParam = (int)data.damageParam;
        var damageType = data.damageType;
        int result;
        switch (damageType)
        {
            case DamageType.normal:
                result = damageParam;
                Debug.Log($"接受到普通攻击{result}");
                return result;
            case DamageType.fired:
                result = damageParam*2;
                Debug.Log($"接受到火焰攻击{result}");
                return result;
            case DamageType.poison:
                result = damageParam/2;
                Debug.Log($"接受到毒攻击{result}");
                return result;
            case DamageType.friendSide:
                Debug.Log($"攻击无效，傷害值{0}");
                return 0;
            default:
                Debug.Log("DamageSystem出Bug了");
                return 0;
        }
    }

    public static void OnUpdate(float deltaTime) 
    {
        
    }

    private static void OnDestroy() 
    {

    }
}
