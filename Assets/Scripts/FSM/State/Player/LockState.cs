using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockState : PlayerRoleState
{
    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnUpdate(float deltaTime)
    {
        //没有按下锁定键，就返回Idle状态
        if (!Role.IsLockPressed) ChangeState<IdleState>();
        //传入玩家角色position
        var target_Gobj = SceneObjManager.GetNearest(role_Gobj.transform.position,SceneObjManager.HookableEntities);
        Role.LockTarget(target_Gobj);
    }
}


///原本的想法：在 lockState中 传入targetGobj调用gamePanel中生成LockHint UI的方法

//新思路：用Action解耦
///状态要控制锁定UI的显示的话，不应该直接引用GamePanel并且直接调用显示锁定UI函数，而是通过GameController 解耦
///因为GameController负责创建 对应UI GamePanel 和 角色，因此肯定可以在某一时刻访问到两者
///这个时候就可以为GamePanel注册一个事件，当进入到锁定状态时 触发这个事件，GamePanel 对应处理就是显示锁定UI
///下面真正有功能的只有gamepanel的LockHint方法

///gamepanel类
///声明LockHint方法（表现层：控制UI激活与配置位置）

///HookableManager类
///声明GetNearest方法（  逻辑层：确定锁定目标）

///  PlayerRole类 
///  public void LockTarget(GameObject target) { OnShowLockTarget?.Invoke(target); }
///  Action<GameObject> OnShowLockTarget（与gamepanel的LockHint方法的 耦合口）

///  LockState类（传参耦合处）
///  Role.LockTarget(target_Gobj)  调用 Action 
  
///解耦
///GameMode1Controller类
///character.OnShowLockTarget += gamePanel.LockHint;
