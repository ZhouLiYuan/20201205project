using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//所有初始化实例交汇处,也是所有实例方法的耦合处
public class GameMode1Controller : GameController
{
    private GamePanel gamePanel;
  
    //private DamageSystem m_damageSystem;

    //private List<GameObject> enemyObjs = new List<GameObject>();
    private PlayerRole m_role;

    Updater m_updater;
    DialogueSystem m_dialogueSystem;
    //InteractableManager interactableManager;

    //需要序列化的敌人名（可能之后需要抽象成一个节点？）
    //public string enemyPrefabName;

    //相当于mono的start()
    public override void StartGame(int level)
    {
        m_updater = Updater.AddUpdater();

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

        //打开GamePanel
        gamePanel = UIManager.Open<GamePanel>();

        //需要在mono Update()中处理的，加多下面的操作
        m_updater.AddUpdateFunction(DamageSystem.OnUpdate);

    }

    public override void ExitGame()
    {
        m_updater.RemoveUpdateFunction(m_dialogueSystem.OnUpdate);
    }

}
