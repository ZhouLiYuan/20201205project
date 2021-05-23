using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 转换状态
/// </summary>
public enum En_Transition
{
    NullTransition = 0,
    LostPlayer,
    SawPlayer,
}

public class En_FSMSystem
{
    private List<EnemyState> en_States;

    public StateID CurrentStateID { get; private set; }

    public EnemyState CurrentState { get; private set; }

    public En_FSMSystem()
    {
        en_States = new List<EnemyState>();
    }

    /// <summary>
    /// 添加新的状态 并更新Current系列字段
    /// </summary>
    public void AddState(EnemyState state)
    {
        if (state == null)
        {
            Debug.LogError("En_FSM状态 引用不能为空");
        }

        //若 状态数组为空 则 Add传入状态 并退出方法
        if (en_States.Count == 0)
        {
            en_States.Add(state);
            CurrentState = state;
            CurrentStateID = state.ID;
            return;
        }

        //避免重复添加同样的State（与集合中的元素对比）
        foreach (EnemyState s in en_States)
        {
            if (s.ID == state.ID)
            {
                Debug.LogError("FSM ERROR: Impossible to add state " + state.ID.ToString() +
                    " because state has already been added");
                return;
            }
        }
        en_States.Add(state);
    }

    /// <summary>
    /// 删除状态
    /// </summary>
    public void DeleteState(StateID id)
    {
        if (id == StateID.NullStateID)
        {
            Debug.LogError("FSM ERROR: NullStateID is not allowed for a real state");
            return;
        }

        foreach (EnemyState state in en_States)
        {
            if (state.ID == id)
            {
                en_States.Remove(state);
                return;
            }
        }
        Debug.LogError("FSM ERROR: Impossible to delete state " + id.ToString() +
                       ". It was not on the list of states");
    }

    /// <summary>
    /// 通过转换，改变FSM的状态
    /// </summary>
    public void PerformTransition(En_Transition trans)
    {
        if (trans == En_Transition.NullTransition)
        {
            Debug.LogError("FSM ERROR: NullTransition is not allowed for a real transition");
            return;
        }
        //获取转换对应的状态ID
        StateID id = CurrentState.GetOutputState(trans);
        if (id == StateID.NullStateID)
        {
            Debug.LogError("FSM ERROR: State " + CurrentStateID.ToString() + " does not have a target state " +
                           " for transition " + trans.ToString());
            return;
        }

        // 更新当前状态ID，currentStateID		
        CurrentStateID = id;
        foreach (EnemyState state in en_States)
        {
            if (state.ID == CurrentStateID)
            {
                // 离开状态时的变量重置
                CurrentState.DoBeforeLeaving();
                // 更新当前状态currentState
                CurrentState = state;
                // 进入状态前，设置进入的状态条件
                CurrentState.DoBeforeEntering();
                break;
            }
        }
    }
}
