using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseState : StateMachineBehaviour
{
    //为敌人提供角色追踪方位
    public GameObject player;
    public PlayerRole role;
    public Transform playerTransform;
   
    public Rigidbody2D en_parent_rb;

    public void Init() 
    {
        role = PlayerManager.m_Role;
        player = PlayerManager.m_Role.GameObject;
        playerTransform = PlayerManager.m_Role.Transform;
    }
}
