using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCState<TRole> : State where TRole : NPC
{
    public TRole NPCRole { get; private set; }
    //lambda表达式写get访问器
    protected GameObject role_Gobj => NPCRole.GameObject;

    protected string triggerName;
    protected string animClipName;

    protected Animator Animator
    {
        get { return NPCRole.animator; }
        set { NPCRole.animator = value; }
    }

    public void SetRole(TRole role)
    {
        NPCRole = role;
        Init();
    }

    public void Init()
    {
        var fullName = GetType().Name;
        triggerName = fullName.Substring(fullName.IndexOf("C")+1, fullName.LastIndexOf("S")-3);
        animClipName = triggerName;
    }

    public override void OnEnter()
    {
        Animator.Play($"{animClipName}", 0);
    }
}
