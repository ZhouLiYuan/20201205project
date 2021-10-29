using UnityEngine;
public class GeneralFSM : FSM
{
    public override TState ChangeState<TState>()
    {
        string stateName = typeof(TState).Name;
        switch (stateName)
        {
            case "IdleState":
            case "MoveState":
            case "JumpState":
            case "DamagedState":
            case "InteractState":
                return ChangeState(stateName) as TState;
            default:
                Debug.Log($"{stateName}不属于{GetType().Name}");
                return CurrentState as TState;
        }
    }
}

public class SubFSM : FSM
{
    public override TState ChangeState<TState>()
    {
        string stateName = typeof(TState).Name;
        switch (stateName)
        {
            case "PreSubActionState":
            case "LockState":
            case "MoveToTargetState":
            case "HookToTargetState":
            case "GetTargetState":
            case "PunchAttackState":
            case "SwordAttackState":
            case "GunAttackState":
                return ChangeState(stateName) as TState;
            default:
                Debug.Log($"{stateName}不属于{GetType().Name}");
                return CurrentState as TState;
        }
    }
}

//原代码

////string IdleState => typeof(IdleState).Name;
//string LockState => typeof(LockState).Name;
//string MoveToTargetState => typeof(MoveToTargetState).Name;

////排除法到State越来越多时就不好用了
//public override TState ChangeState<TState>()
//{
//    string stateName = typeof(TState).Name;
//    if (stateName != LockState && stateName != MoveToTargetState) return ChangeState(stateName) as TState;
//    else
//    {
//        Debug.Log($"{stateName}不属于{GetType().Name}");
//        return CurrentState as TState;
//    }
//}