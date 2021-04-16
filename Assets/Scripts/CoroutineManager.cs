using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineManager 
{
    //套娃不会引发递归吗？
    internal class CoroutineRunner : MonoBehaviour { }

    //静态构造函数
    static CoroutineManager()
    {
        var obj = new GameObject();
        m_coroutineRunner = obj.AddComponent<CoroutineRunner>();
    }

    internal static CoroutineRunner m_coroutineRunner;

    //传入枚举器
    public static void StarCoroutine(IEnumerator enumerator)
    {
        m_coroutineRunner.StartCoroutine(enumerator);
    }

}
