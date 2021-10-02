using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractState : PlayerRoleState
{
    NPCInteractablePanel t = new NPCInteractablePanel();

    public override void OnUpdate(float deltaTime)
    {
        t.state = NPCInteractablePanel.State.Finish;
        //结束交互时返回(比如在每个InteractPanel里都放一个对应的State或者写一个事件通知) 待机状态
        if (false) ChangeState<IdleState>();
        //传入玩家角色position 获取距离玩家最近的交互物(或者追加一个按下方向键可 切换交互对象的方法)
        var target_Gobj = SceneObjManager.GetNearest(role_Gobj.transform.position, SceneObjManager.InteractableEntities);
        Role.Interact(target_Gobj);

    }
}
