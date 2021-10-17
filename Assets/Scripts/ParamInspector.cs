using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using Sirenix.OdinInspector.Editor.Modules;

//可以用静态做成单例？
//主要是负责在游戏进行（也就是Update的时候动态设置数据）
public class ParamInspector : SerializedMonoBehaviour
{
    //需要加载的角色名称(序列化),应该在start里调用
    public string RoleName
    {
        get { return PlayerManager.m_RoleName; }
        private set { PlayerManager.m_RoleName = value; }
    }

    //获取玩家角色实例
    public PlayerRole Role
    {
        get { return PlayerManager.roles[this.gameObject]; }
    }


    //这种层层包装的有个难点（从哪获得实例？）
    //目前设计：把实例都放在PlayerManager里

    //设置玩家移动速度（MoveState）
    public float MoveSpeed
    {
        get { return Role.moveSpeed; }
        set { Role.moveSpeed = value; }
    }

    //跳跃速度同理
    public float JumpSpeed
    {
        get { return Role.jumpSpeed; }
        set { Role.jumpSpeed = value; }
    }



    public void Init()
    {

    }

}
