using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using Spine;
using Spine.Unity;


//还充当一些Animator的作用
public class Spine_GeneralFSM : FSM
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
                return ChangeState(stateName) as TState;
            default:
                Debug.Log($"{stateName}不属于{GetType().Name}");
                return CurrentState as TState;
        }
    }
}

public class Spine_AttackFSM : FSM
{
    public override TState ChangeState<TState>()
    {
        string stateName = typeof(TState).Name;
        switch (stateName)
        {
            case "PreSubActionState":
            case "LightPunchState":
            case "MediumPunchState":
            case "LightKickState":
            case "MediumKickState":
                return ChangeState(stateName) as TState;
            default:
                Debug.Log($"{stateName}不属于{GetType().Name}");
                return CurrentState as TState;
        }
    }


}

#region 作为一种快速打case的手段(暂时用不上)
//public enum RoleState
//{
//    Idle,
//    Move,
//    Damaged,
//    Attack

//    //idleAnim,
//    //moveAnim,
//    //jumpAnim,
//    //attackAnim
//}
//public RoleState roleState;


//也可以不用枚举
//switch (trackEntry.Animation.Name)
//{
//    case idleAnim:
//        break;
//    case moveAnim:
//        break;
//    case jumpAnim:
//        break;
//    case attackAnim:
//        break;
//    default:
//        break;
//}
//SetAnimation(nextAnimation);
//switch (roleState)
//{
//    case RoleState.Idle:
//        break;
//    case RoleState.Move:
//        break;
//    case RoleState.Damaged:
//        break;
//    case RoleState.Attack:
//        break;
//    default:
//        break;
//}

#endregion



