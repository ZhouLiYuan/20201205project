using UnityEngine;
public class HookFSM : FSM
{
    public override TState ChangeState<TState>() 
    {
        string stateName = typeof(TState).Name;
        switch (stateName) 
        {
            case "IdleState":
            case "LockState":
            case "MoveToTargetState":
                return ChangeState(stateName) as TState;
            default:
                Debug.Log($"{stateName}不属于{GetType().Name}");
                return CurrentState as TState;
        }
    }
}
