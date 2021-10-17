using UnityEngine;
public class GeneralFSM : FSM
{
    //string IdleState => typeof(IdleState).Name;
    string LockState => typeof(LockState).Name;
    string MoveToTargetState => typeof(MoveToTargetState).Name;


    public override TState ChangeState<TState>() 
    {
        string stateName = typeof(TState).Name;
        if (stateName != LockState && stateName != MoveToTargetState) return ChangeState(stateName) as TState;
        else 
        {
            Debug.Log($"{stateName}不属于{GetType().Name}");
            return CurrentState as TState;
        }
    }
}
