//Unity Wiki状态机教程 Transition枚举和 相关处理画蛇添足



//using System;
//using System.Collections.Generic;
//using UnityEngine;




///// <summary>
///// 状态ID
///// </summary>
//public enum StateID
//{
//    NullStateID = 0,
//    FollowingPath,
//    ChasingPlayer,
//}

///// <summary>
///// 有限状态机系统中的状态
///// 每个状态都有一个字典，字典中有键值对(转换-状态),保存转换Key时，把枚举转换为int，作为key
///// 表示如果在当前状态下触发转换，那么FSM应该处于对应的状态。
///// </summary>
//public abstract class EnemyState
//{
//    protected Dictionary<int, StateID> m_Map = new Dictionary<int, StateID>();
//    protected StateID stateID;
//    public StateID ID { get { return stateID; } }

//    /// <summary>
//    /// 添加转换
//    /// </summary>
//    public void AddTransition(En_Transition trans, StateID id)
//    {
//        //先做非空判断
//        if (trans == En_Transition.NullTransition)
//        {
//            Debug.LogError("EnemyState ERROR: 不允许将 NullTransition作为 状态ID");
//            return;
//        }
//        if (id == StateID.NullStateID)
//        {
//            Debug.LogError("EnemyState ERROR: 不允许将 NullStateID作为 状态ID");
//            return;
//        }

//        //检查有没有重复的转换？
//        int transition = (int)trans;
//        if (m_Map.ContainsKey(transition))
//        {
//            Debug.LogError("EnemyState ERROR:  " + stateID.ToString() + "状态已经转换 " + trans.ToString() +
//                           "无法配置到另外的状态");
//            return;
//        }

//        m_Map.Add(transition, id);
//    }

//    /// <summary>
//    /// 删除转换
//    /// </summary>
//    public void DeleteTransition(En_Transition trans)
//    {
//        if (trans == En_Transition.NullTransition)
//        {
//            Debug.LogError("EnemyState ERROR: 转换不能为空");
//            return;
//        }

//        //先把枚举转换成int
//        int transition = (int)trans;
//        if (m_Map.ContainsKey(transition))
//        {
//            m_Map.Remove(transition);
//            return;
//        }
//        Debug.LogError("EnemyState ERROR:  " + trans.ToString() + " passed to " + stateID.ToString() +
//                       " was not on the state's transition list");
//    }

//    /// <summary>
//    /// 根据转换返回状态ID
//    /// </summary>
//    public StateID GetOutputState(En_Transition trans)
//    {
//        int transition = (int)trans;
//        if (m_Map.ContainsKey(transition))
//        {
//            return m_Map[transition];
//        }
//        return StateID.NullStateID;
//    }

//    /// <summary>
//    /// 用于进入状态前，设置进入的状态条件
//    /// 在进入当前状态之前，FSM系统会自动调用
//    /// </summary>
//    public virtual void DoBeforeEntering() { }

//    /// <summary>
//    /// 用于离开状态时的变量重置
//    /// 在更改为新状态之前，FSM系统会自动调用
//    /// </summary>
//    public virtual void DoBeforeLeaving() { }

//    /// <summary>
//    /// 用于判断是否可以转换到另一个状态,每帧都会执行
//    /// </summary>
//    public abstract void CheckTransition(GameObject player, GameObject npc);

//    /// <summary>
//    /// 控制NPC行为,每帧都会执行
//    /// </summary>
//    public abstract void Act(GameObject player, GameObject npc);

//}