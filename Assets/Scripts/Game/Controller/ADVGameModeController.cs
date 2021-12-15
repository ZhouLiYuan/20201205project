using Role.SelectableRole;
using UnityEngine;

//所有初始化实例交汇处,也是所有实例方法的耦合处
public class ADVGameModeController : GameController
{
    private AdvGamePanel gamePanel;

    private PlayerRole m_role;

    Updater m_updater;
    DialogueSystem m_dialogueSystem;

    //需要序列化的敌人名（可能之后需要抽象成一个节点？）
    //public string enemyPrefabName;

    //相当于mono的start()
    public override void StartGame(int level)
    {
        //加载主角
        m_role = PlayerManager.SpawnPlayer1();
        //控制器和角色耦合
        PlayerInput m_playerInput = new PlayerInput();//创建玩家 控制器
        m_playerInput.InitInput();
        m_role.BindInput(m_playerInput);

        //初始化关卡场景和相机
        CameraManager.InitCamera();
        CameraManager.SetAdvModeAim(m_role.Transform);

        AdvLevelManager.InitLevel(level);

        //打开GamePanel
        gamePanel = UIManager.OpenPanel<AdvGamePanel>();

        m_updater = Updater.AddUpdater();
        //mono Update()中处理的，加多下面的操作
        m_updater.AddUpdateFunction(DamageSystem.OnUpdate);


        //odin调节面板(必须放到最后,防止相关引用还未被初始化，比如PlayerRole)
        var OdinInspector = new GameObject();
        OdinInspector.name = "RuntimeOdinInspector";
        OdinInspector.AddComponent<RuntimeOdinInspector>();
    }

    public override void ExitGame()
    {
        m_updater.RemoveUpdateFunction(m_dialogueSystem.OnUpdate);
    }

}
