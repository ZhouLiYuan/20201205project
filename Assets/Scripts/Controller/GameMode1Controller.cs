﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameMode1Controller  : GameController
{
    private GamePanel gamePanel;

    //private List<GameObject> enemyObjs = new List<GameObject>();
    private PlayerRole m_role;


    //需要序列化的敌人名（可能之后需要抽象成一个节点？）
    //public string enemyPrefabName;


    public override void StartGame(int level)
    {
        //打开GamePanel
        gamePanel = UIManager.Open<GamePanel>();

        //创建玩家 控制器
        PlayerInput m_playerInput = new PlayerInput();
        m_playerInput.InitInput();

        //加载主角
        m_role = PlayerManager.SpawnCharacter();
        //控制器和角色耦合
        m_role.BindInput(m_playerInput);

        //初始化关卡场景和相机
        CameraManager.InitCamera(m_role);
        LevelManager.InitLevel(level);

        //耦合Lock UI逻辑和 PlayerRole功能
        m_role.OnShowLockTarget += gamePanel.LockHint;
    }

    public override void ExitGame()
    {
        m_role.OnShowLockTarget -= gamePanel.LockHint;
    }





  
}