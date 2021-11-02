using Role.SelectableRole;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester :MonoBehaviour
{
    private PlayerRole m_role;

    Updater m_updater;

    private void Start()
    {
        m_updater = Updater.AddUpdater();

        //创建玩家 控制器
        PlayerInput m_playerInput = new PlayerInput();
        m_playerInput.InitInput();
        //加载主角
        m_role = PlayerManager.SpawnCharacter();
        //控制器和角色耦合
        m_role.BindInput(m_playerInput);

        LevelManager.InitByConfiguration(0);
    }
}
