using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//做一个抽象不可见的hookable Gobj来作为可抓取物的识别标签
public class SceneObjManager
{
    public static List<GameObject> HookableEntities = new List<GameObject>();
    public static List<GameObject> InteractableEntities = new List<GameObject>();

    public static void InitHookableEntities(IEnumerable<GameObject> objs)
    {
        HookableEntities.AddRange(objs);
    }

    public static void InitInteractableEntities(IEnumerable<GameObject> objs)
    {
        HookableEntities.AddRange(objs);
    }

    /// <summary>
    /// 获取最近目标
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public static GameObject GetNearest(Vector3 position, List<GameObject> Entities)
    {
        if (Entities.Count == 0) return null;
        //用集合中的第一个元素 初始化 result
        GameObject result = Entities[0];
        //SqrMagnitude取模的平方(相减得出的向量是 玩家 和 最近敌人的距离)
        //同样用第一个元素 到玩家的距离 初始化 minDistance
        float minDistance = Vector3.SqrMagnitude(position - result.transform.position);
       
        for (int i = 1; i < Entities.Count;i++ ) 
        {
            var Gobj = Entities[i];
            var currentDistance = Vector3.SqrMagnitude(position - Gobj.transform.position);
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
