using UnityEngine;
public class AttackFSM : FSM
{
    public override TState ChangeState<TState>() 
    {
        string stateName = typeof(TState).Name;
        switch (stateName) 
        {
            case "PreAttackState":
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
