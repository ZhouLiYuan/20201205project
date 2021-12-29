using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//做一个抽象不可见的hookable Gobj来作为可抓取物的识别标签
public class ADVSceneGobjManager
{
    public static List<GameObject> HookableEntities = new List<GameObject>();
    public static List<GameObject> InteractableEntities = new List<GameObject>();

    private static void MakeZAxisZero(GameObject obj)
    {
        Vector3 posWithoutZ = obj.transform.position;
        posWithoutZ.z *= 0f;
        obj.transform.position = posWithoutZ;
    }

    public static void InitHookableEntities(IEnumerable<GameObject> objs)
    {
        foreach (var hookableItem in objs){MakeZAxisZero(hookableItem);}
        HookableEntities.AddRange(objs);
    }

    public static void InitInteractableEntities(IEnumerable<GameObject> objs)
    {
        foreach (var InteractableItem in objs) { MakeZAxisZero(InteractableItem); }
        InteractableEntities.AddRange(objs);
    }

    /// <summary>
    /// 获取最近目标
    /// </summary>
    /// <param name="centerPos"></param>
    /// <returns></returns>
    public static GameObject GetNearest(Vector3 centerPos, List<GameObject> EntitiesNeedToCompare)
    {
        if (EntitiesNeedToCompare.Count == 0) return null;
        //用集合中的第一个元素 初始化 result
        GameObject result = EntitiesNeedToCompare[0];
        var firstTargetPosition = result.transform.position;
        firstTargetPosition.z = 0f;
        centerPos.z = 0f; //计算距离时不应该包含z轴
        //SqrMagnitude取模的平方(相减得出的向量是 玩家 和 最近敌人的距离)
        //同样用第一个元素 到玩家的距离 初始化 minDistance
        float minDistance = Vector3.SqrMagnitude(centerPos - firstTargetPosition);
       
        for (int i = 1; i < EntitiesNeedToCompare.Count;i++ ) 
        {
            var Gobj = EntitiesNeedToCompare[i];
            var targetPosition = Gobj.transform.position;
            targetPosition.z = 0f;
            var currentDistance = Vector3.SqrMagnitude(centerPos - targetPosition);
            //如果新的距离比当前较小，就覆盖上一个result
            //注意这里的本地变量位置的放置设计（本地变量声明周期出不了自己所在的{}），result的声明周期比Gobj长
            if (currentDistance < minDistance)
            {
                minDistance = currentDistance;
                result = Gobj;
            }
        }
        return result;
    }

}
