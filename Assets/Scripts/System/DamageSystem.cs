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
        int atkParam = (int)data.atkValue;
        int defParam = (int)data.defValue;
        var damageType = data.damageType;


        int result;
        switch (damageType)
        {
            case AdvDamageType.normal:
                result = atkParam - defParam;
                Debug.Log($"接受到普通攻击{result}");
                return result;
            case AdvDamageType.fired:
                result = atkParam* 2 - defParam;
                Debug.Log($"接受到火焰攻击{result}");
                return result;
            case AdvDamageType.poison:
                result = atkParam/2;
                Debug.Log($"接受到毒攻击{result}");
                return result;
            case AdvDamageType.friendSide:
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
