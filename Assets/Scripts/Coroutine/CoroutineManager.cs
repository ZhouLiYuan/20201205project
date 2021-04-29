using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 功能：让mono中的协程API 能够以静态的方式 被调用
/// 实现静态版本的 协程API
/// </summary>
public static class CoroutineManager 
{
    /// <summary>
    /// mono类 CoroutineManager内嵌类
    /// </summary>
    internal class CoroutineRunner : MonoBehaviour { }

    internal static CoroutineRunner m_coroutineRunner;

    //静态构造函数
    static CoroutineManager()
    {
        var obj = new GameObject();
        m_coroutineRunner = obj.AddComponent<CoroutineRunner>();
    }



    /// <summary>
    /// 传入枚举器,开启协程
    /// </summary>
    /// <param name="enumerator"></param>
    /// <returns></returns>
    public static Coroutine StartCoroutine(IEnumerator enumerator)
    {
       return m_coroutineRunner.StartCoroutine(enumerator);
    }


    /// <summary>
    /// 停止协程
    /// </summary>
    /// <param name="coroutine"></param>
    public static void StopCoroutine(Coroutine coroutine)
    {
        m_coroutineRunner.StopCoroutine(coroutine);
    }

}
