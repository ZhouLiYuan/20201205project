using System;
using System.Collections.Generic;
using UnityEngine;


//HookToTarget以基类的方式作为前状态
public class HookToTargetState : HookBaseState
{
    public override void OnEnter()
    {
        //没有动画clip的一个状态 所以不要调用base.OnEnter();
    }

    public override void OnUpdate(float deltaTime)//如果途中遇到障碍物就会打断取消返回默认状态
    {
        base.OnUpdate(deltaTime);
      
         direction = role_Gobj.transform.position - TargetPos;
        HookGobj.transform.position = Vector2.MoveTowards(HookPos,TargetOwnerPos,1f);//1f钩锁的移动速度
        //HookToTargetDistance = (float)Math.Sqrt((HookPos  - TargetPos).sqrMagnitude); //先平方在开平方根
        

        //最合乎逻辑的可能还是在Hook上挂个MONO脚本，用OnCollision去检测 (也能省掉很多距离判断！！！！！！！！！)
        //var result = Physics2D.CircleCast(hookGobjPos, 0.5f, direction, HookToTargetDistance);//待检验
        var result = Physics2D.Linecast(HookPos, TargetPos);//注意要给Hookable标签Gobj添加碰撞体，手动设置好碰撞层

        if (result.collider == null) return;
        else if (result.collider.gameObject.tag == TagManager.Hookable || result.collider.gameObject.tag == TagManager.Enemy)//这样逻辑TagGobj的collider必须要大
        {
            HookToTargetDistance = Vector2.Distance(HookPos, TargetPos);
            if (HookToTargetDistance > 0.5f)//判断hook是否在目标附近
            {
                //Role.hookGobj.transform.position = hookGobjPos;//这种方法可能会有穿墙bug
                return;
            }
            
            GenerateHookEffect(result.point);
            //var targetParentLayer = Role.lockTarget.GetComponentInParent<Transform>().gameObject.layer;
            var targetParentLayer = TargetOwnerTransform.gameObject.layer;
            if (targetParentLayer == LayerMask.NameToLayer(LayerManager.Platform) /*&& Role.GroundDetect.IsGrounded*/)
            {
                ChangeState<MoveToTargetState>();
            }
            else if (targetParentLayer == LayerMask.NameToLayer(LayerManager.Enemy))
            {
                ChangeState<GetTargetState>();//暂定所有Enemy都可抓取
            }
            HookGobj.transform.position = TargetPos;
        }
        else
        {
            if ((Vector2)HookPos != result.point) return;//不严谨的算法，直到碰到障碍物hook才停下
            GenerateHookEffect(result.point);
            ChangeState<PreSubActionState>();
            Role.lockTarget = null;
            Role.hookGobj.transform.position = Role.Transform.position + Role.hookLocaloffsetPosSlash;//Hook回归原位
        } 


    }

    public override void OnExit()
    {

    }
}

