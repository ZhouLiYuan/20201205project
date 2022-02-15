using Role;
using Role.SelectableRole;
using Sirenix.OdinInspector;
using Role.SpineRole;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BTL_RuntimeOdinInspector : SerializedMonoBehaviour
{
    private void Start()//先手动把必须初始化的部分初始化一下
    {
        SynchronizePlayerRole();
        SynchronizeBoxesDic();
    }

    [TabGroup("参战角色")]
    [ShowInInspector, Searchable]
    [PropertyOrder(0)]//属性顺序
    [InlineButton("SynchronizePlayerRole", "追加同步设置")]
    private PlayerRole_Spine player1,player2;
    private void SynchronizePlayerRole() //确保每次只会序列化一个(防止P3 P4空引用)
    {
        if (player1 == null) 
        {
            player1 = BtlCharacterManager.p1_Role;
            return;
        }
        if (player2 == null)
        {
            player2 = BtlCharacterManager.p2_Role;
            return;
        }
        return;
    }


    //字典（运行时）
    [TabGroup("HitBox")]
    [GUIColor(1, .5f, 0, 1)]//橙色
    [ShowInInspector, Searchable]
    [InlineButton("SynchronizeBoxesDic", "同步设置")]
    [DictionaryDrawerSettings]
    private Dictionary<Collider2D, PlayerRole_Spine> boxesDic;
    private void SynchronizeBoxesDic() { boxesDic = HitBoxManager.boxesDic; }

}
