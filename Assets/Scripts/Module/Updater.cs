using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 使用<see cref="MonoBehaviour"/>的
/// <see cref="Update"/>，在<see cref="Update"/>时机执行所有<see cref="updateActions"/>委托
/// <see cref="FixedUpdate"/>,在<see cref="FixedUpdate"/>时机执行所有<see cref="fixedUpdateActions"/>委托
/// </summary>
public class Updater : MonoBehaviour
{
    //update时需要调用的 方法集合(Add Remove)
    private readonly List<Action> updateActions = new List<Action>();
    private readonly List<Action> toAddUpdateActions = new List<Action>();
    private readonly List<Action> toRemoveUpdateActions = new List<Action>();

    //float类型参数WithDeltaTime委托版本 
    private readonly List<Action<float>> updateActionsWithDeltaTime = new List<Action<float>>();
    private readonly List<Action<float>> toAddUpdateActionsWithDeltaTime = new List<Action<float>>();
    private readonly List<Action<float>> toRemoveUpdateActionsWithDeltaTime = new List<Action<float>>();

    //FixedUpdate版本
    private readonly List<Action> fixedUpdateActions = new List<Action>();
    private readonly List<Action> toAddFixedUpdateActions = new List<Action>();
    private readonly List<Action> toRemoveFixedUpdateActions = new List<Action>();

    private readonly List<Action<float>> fixedUpdateActionsWithDeltaTime = new List<Action<float>>();
    private readonly List<Action<float>> toAddFixedUpdateActionsWithDeltaTime = new List<Action<float>>();
    private readonly List<Action<float>> toRemoveFixedUpdateActionsWithDeltaTime = new List<Action<float>>();

    public object m_target;
    private static GameObject m_Gobj;

    //由.Net调用，创建表现层
    static Updater()
    {
        //建立updater Gobj逻辑层和表现层联系
        DontDestroyOnLoad(m_Gobj = new GameObject(nameof(Updater)));
    }

    
    void Update()
    {
        #region Add
        if (toAddUpdateActions.Count > 0)
        {
            foreach (var action in toAddUpdateActions) updateActions.Add(action);
            toAddUpdateActions.Clear();
        }
        if (toAddUpdateActionsWithDeltaTime.Count > 0)
        {
            foreach (var action in toAddUpdateActionsWithDeltaTime) updateActionsWithDeltaTime.Add(action);
            toAddUpdateActionsWithDeltaTime.Clear();
        }
        if (toAddFixedUpdateActions.Count > 0)
        {
            foreach (var action in toAddFixedUpdateActions) fixedUpdateActions.Add(action);
            toAddFixedUpdateActions.Clear();
        }
        if (toAddFixedUpdateActionsWithDeltaTime.Count > 0)
        {
            foreach (var action in toAddFixedUpdateActionsWithDeltaTime) fixedUpdateActionsWithDeltaTime.Add(action);
            toAddFixedUpdateActionsWithDeltaTime.Clear();
        }
        #endregion

        #region Remove
        //把 “需要Remove的Action数组集合（列表）” 中的action元素取出，传入updateActions用Remove方法挨个移除（add同理）
        if (toRemoveUpdateActions.Count > 0)
        {
            foreach (var action in toRemoveUpdateActions) updateActions.Remove(action);
            //清空 “待Remove列表”
            toRemoveUpdateActions.Clear();
        }
        if (toRemoveUpdateActionsWithDeltaTime.Count > 0)
        {
            foreach (var action in toRemoveUpdateActionsWithDeltaTime) updateActionsWithDeltaTime.Remove(action);
            toRemoveUpdateActionsWithDeltaTime.Clear();
        }
        if (toRemoveFixedUpdateActions.Count > 0)
        {
            foreach (var action in toRemoveFixedUpdateActions) fixedUpdateActions.Remove(action);
            toRemoveFixedUpdateActions.Clear();
        }
        if (toRemoveFixedUpdateActionsWithDeltaTime.Count > 0)
        {
            foreach (var action in toRemoveFixedUpdateActionsWithDeltaTime) fixedUpdateActionsWithDeltaTime.Remove(action);
            toRemoveFixedUpdateActionsWithDeltaTime.Clear();
        }
        #endregion

        //调用updateActions里的所有委托
        foreach (var action in updateActions) action();
        foreach (var action in updateActionsWithDeltaTime) action(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        foreach (var action in fixedUpdateActions) action();
        foreach (var action in fixedUpdateActionsWithDeltaTime) action(Time.fixedDeltaTime);
    }



    /// <summary>
    /// 生成一个 <see cref="Updater"/>组件 实例
    /// <para>调用Destroy方法来销毁并停止<see cref="Updater"/></para>
    /// </summary>
    /// <param name="isVisible"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public static Updater AddUpdater(bool isVisible = false, object target = null) 
    {
        //全局只有一个叫Updater的Gobj实例 但是 同一个Gobj上可以有多个 同名Updater的Component（对应不同target Gobj（看是否有传入target可选参数））
        //因为component并不是全局唯一的，所以建立表现层和逻辑层的耦合 不在静态构造函数内进行
        Updater updater = m_Gobj.AddComponent<Updater>();
        //建立Updater组件中Target成员 和对应TargetGobj（场景中需要参与Update（）的Gobj） 表现层和逻辑层的耦合
        updater.m_target = target;
        if (!isVisible) updater.hideFlags = HideFlags.HideInInspector;
        return updater;
    }

    //---------------List<Action>相关方法(Add Remove)--------------------------------------

    /// <summary>
    /// 添加需要被Update的委托
    /// </summary>
    /// <param name="action"></param>
    public void AddUpdateFunction(Action action)
    {
        toAddUpdateActions.Add(action);
    }

    /// <summary>
    /// 添加需要被Update的委托
    /// </summary>
    /// <param name="action"></param>
    public void AddUpdateFunction(Action<float> action)
    {
        toAddUpdateActionsWithDeltaTime.Add(action);
    }

    /// <summary>
    /// 添加需要被FixedUpdate的委托
    /// </summary>
    /// <param name="action"></param>
    public void AddFixedUpdateFunction(Action action)
    {
        toAddFixedUpdateActions.Add(action);
    }

    /// <summary>
    /// 添加需要被FixedUpdate的委托
    /// </summary>
    /// <param name="action"></param>
    public void AddFixedUpdateFunction(Action<float> action)
    {
        toAddFixedUpdateActionsWithDeltaTime.Add(action);
    }

    /// <summary>
    /// 移除Update中的委托
    /// </summary>
    /// <param name="action"></param>
    public void RemoveUpdateFunction(Action action)
    {
        toRemoveUpdateActions.Add(action);
    }

    /// <summary>
    /// 移除Update中的委托
    /// </summary>
    /// <param name="action"></param>
    public void RemoveUpdateFunction(Action<float> action)
    {
        toRemoveUpdateActionsWithDeltaTime.Add(action);
    }

    /// <summary>
    /// 移除FixedUpdate中的委托
    /// </summary>
    /// <param name="action"></param>
    public void RemoveFixedUpdateFunction(Action action)
    {
        toRemoveFixedUpdateActions.Add(action);
    }

    /// <summary>
    /// 移除FixedUpdate中的委托
    /// </summary>
    /// <param name="action"></param>
    public void RemoveFixedUpdateFunction(Action<float> action)
    {
        toRemoveFixedUpdateActionsWithDeltaTime.Add(action);
    }


}
