using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UItool 
{
    /// <summary>
    /// 当前激活的面板
    /// </summary>
    private static GameObject activePanel;

    ///// <summary>
    ///// 新建实例时 构造函数 传参 初始化字段
    ///// </summary>
    ///// <param name="panel"></param>
    //public static UItool(GameObject panel)
    //{
    //    activePanel = panel;
    //}




    /// <summary>
    /// 通过 类型参数 获取当前面板一个组件 
    /// 若不存在 该类型组件 就添加一个 再获取
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T GetOrAddComponent<T>() where T : Component 
    {
        if (activePanel.GetComponent<T>() == null) { activePanel.AddComponent<T>(); }

        return activePanel.GetComponent<T>();
    }

    /// <summary>
    /// 根据名称查找一个 子对象
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static GameObject FindChildGameObject(string name) 
    {

        //为什么不能用findGameObject根据具体名字来找子对象？
        //return activePanel.transform.Find($"{ name}").GetComponent<Transform>().gameObject;
        
        //获取所有子对象Transform组件
        Transform[ ] trans = activePanel.GetComponentsInChildren<Transform>();

        foreach (Transform item in trans)
        {
            if (item.name == name) { return item.gameObject; }
        }

        Debug.LogWarning($"淦！{activePanel.name}里没有叫做{name}的子对象啊");
        return null;
    }

    /// <summary>
    ///根据名称获取一个对象的组件(没有就先添加再获取)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <returns></returns>
    public static T GetOrAddComponentInChildren<T>(string name) where T : Component
    {
        GameObject child = FindChildGameObject(name);
        if (child)
        {
            if (child.GetComponent<T>() == null) 
            { child.AddComponent<T>(); }

            return child.GetComponent<T>();
        }

        Debug.Log($"并没有叫做{name}的子物体");
        return null;
    }
}
