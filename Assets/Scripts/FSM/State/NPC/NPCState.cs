using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCState<TRole> : State where TRole : NPC
{
    public TRole NPCRole { get; private set; }
    //lambda表达式写get访问器
    protected GameObject role_Gobj => NPCRole.GameObject;

    public void SetRole(TRole role)
    {
        NPCRole = role;
    }
}
