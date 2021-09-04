using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSystem
{
   


    public void Init() 
    {
        
    }

   
    /// <summary>
    /// 返回值是计算后得出的傷害值
    /// </summary>
    /// <returns></returns>
    public float CalculateDamage(DamageData data) 
    {
        float damageParam=data.damageParam;
        var damageType = data.damageType;
        float result;
        switch (damageType)
        {
            case DamageData.DamageType.normal:
                result = damageParam;
                Debug.Log($"接受到普通攻击{result}");
                return result;
            case DamageData.DamageType.fired:
                result = damageParam*2;
                Debug.Log($"接受到火焰攻击{result}");
                return result;
            case DamageData.DamageType.poison:
                result = damageParam/2;
                Debug.Log($"接受到毒攻击{result}");
                return result;
            case DamageData.DamageType.friendSide:
                Debug.Log($"攻击无效，傷害值{0}");
                return 0;
            default:
                Debug.Log("DamageSystem出Bug了");
                return 0;
              
        }
    }

    public void OnUpdate(float deltaTime) 
    {
        
    }

    private void OnDestroy() 
    {

    }
}
