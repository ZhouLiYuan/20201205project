using System;
using System.Collections.Generic;
using UnityEngine;

//框架性质脚本一般 方法 会更完备（不能算啰嗦）
//提供同名同功能（多参数版本）方法：
//泛型方法（类型参数），非泛型（多参数列表重载）

/// <summary>
/// FSM管理 所有的状态
/// 需要精准的返回类型时推荐 泛型方法
/// </summary>
public class FSM
{

    public State PreviousState { get; protected set; }
    public State CurrentState { get; protected set; }
    public State NextState { get; protected set; }
    protected readonly Dictionary<string, State> States = new Dictionary<string, State>();

    //------------------------------------------<添加状态>----------------------------------------

    /// <summary>
    /// 用类型参数创建实例
    /// </summary>
    /// <typeparam name="TState"></typeparam>
    /// <returns></returns>
    public TState AddState<TState>() where TState : State,new()
    {
        string stateName = typeof(TState).Name;
        var state = new TState();
        state.InitFSM(this);

        if (States.Count == 0)
        {
            PreviousState  = CurrentState;
            CurrentState = state;
            NextState = null;
        }

        States[stateName] = state;

        return state;
    }

    /// <summary>
    /// 用类型名（Type就是class的数据类型），创建状态实例
    /// 但返回值只能是基类State
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public State AddState(Type type)
    {
        //Activator.CreateInstance：传入类名称type 返回一个该类实例
        var state = Activator.CreateInstance(type) as State;
        state.InitFSM(this);

        if (States.Count == 0)
        {
            PreviousState = CurrentState = CurrentState;
            CurrentState = state;
            NextState = null;
        }

        States[type.Name] = state;
        return state;

    }

    //------------------------------------------<移除状态>----------------------------------------

    public TState RemoveState<TState>() where TState : State
    {
        var name = typeof(TState).Name;
        State state;
        //TryGetValue用于查找不确定数据
        if (States.TryGetValue(name, out state))
        {
            States.Remove(name);
            //用as做类型转换（较为安全）
            return state as TState;
        }
        else return null;
    }

    public State RemoveState(string stateName)
    {
        State state;
        if (States.TryGetValue(stateName, out state))
        {
            States.Remove(stateName);
            return state;
        }
        return null;
    }

    //------------------------------------------<改变状态>----------------------------------------
    //三合一版本，因为是框架代码，所以希望能拆分出应对 多参数类型的同名方法
    //public TState ChangeState<TState>() where TState : State
    //{
    //    string stateName = typeof(TState).Name;
    //    State state = null;
    //    State nextState;
    //    //根据stateName找到状态nextState
    //    if (States.TryGetValue(stateName, out nextState))
    //    {
    //        state = nextState;
    //    }
    //    else Debug.LogError($"FSM的字典中没有{stateName}状态");

    //    return state as TState;
    //}

    public virtual TState ChangeState<TState>() where TState : State
    {
        string stateName = typeof(TState).Name;
        return ChangeState(stateName) as TState;
    }


    //TryGetValue和ContainKey 性能对比https://zhuanlan.zhihu.com/p/104681735
    //查找不确定数据的时候应该用TryGetValue（且性能更好比ContainKey少调用一次FindEntry）
    public State ChangeState(string stateName)
    {
        //Debug.Log($"当前FSM为{this}");
        State state = null;
        if (States.TryGetValue(stateName, out var nextState)) state = ChangeState(nextState);
        else {Debug.LogError($"FSM的字典中没有{stateName}状态");}
        return state;
    }

    public State ChangeState(State nextState)
    {
        return NextState = nextState;
    }

    //------------------------------------------<刷新功能>----------------------------------------

    public void Update(float deltaTime)
    {
        //如果当前State不为空 又已经有下一个准备转换的NextState
        if (NextState != null && CurrentState != null)
        {
            CurrentState.OnExit();
            PreviousState = CurrentState;
            CurrentState = NextState;
            NextState = null;
            CurrentState.OnEnter();
        }
        CurrentState?.OnUpdate(deltaTime);
    }

    //FixedUpdate不是在指定时间点刷新，而是每隔固定秒数刷新，所以不需要做状态字段的配置
    public void FixedUpdate(float fixedDeltaTime)
    {
        CurrentState?.OnFixedUpdate(fixedDeltaTime);
    }

}
