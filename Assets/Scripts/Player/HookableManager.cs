using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookableManager
{
    private static List<GameObject> entities = new List<GameObject>();

    public static void Init(IEnumerable<GameObject> objs)
    {
        entities.AddRange(objs);
    }


    /// <summary>
    /// 获取最近目标
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public static GameObject GetNearest(Vector3 position)
    {
        if (entities.Count == 0) return null;
        //用集合中的第一个元素 初始化 result
        GameObject result = entities[0];
        //SqrMagnitude取模的平方(相减得出的向量是 玩家 和 最近敌人的距离)
        //同样用第一个元素 到玩家的距离 初始化 minDistance
        float minDistance = Vector3.SqrMagnitude(position - result.transform.position);
       
        for (int i = 1; i < entities.Count;i++ ) 
        {
            var Gobj = entities[i];
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
