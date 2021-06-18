using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameMode1Controller  : GameController
{
    private GamePanel gamePanel;

    private List<GameObject> enemyObjs = new List<GameObject>();
    private PlayerRole m_role;


    //需要序列化的敌人名（可能之后需要抽象成一个节点？）
    public string enemyPrefabName;


    public override void StartGame(int level)
    {
        //打开GamePanel
        gamePanel = UIManager.OpenPanel<GamePanel>();

        //创建玩家 控制器
        PlayerInput m_playerInput = new PlayerInput();
        m_playerInput.InitInput();

        //加载主角
        m_role = PlayerManager.SpawnCharacter();
        //控制器和角色耦合
        m_role.BindInput(m_playerInput);

        //初始化关卡场景和相机
        InitCamera(m_role);
        InitLevel(level);

        //耦合Lock UI逻辑和 PlayerRole功能
    }

    public override void ExitGame()
    {
        m_role.OnShowLockTarget -= gamePanel.LockHint;
    }


    /// <summary>
    /// 初始化相机
    /// </summary>
    private void InitCamera(PlayerRole role) 
    {
        //设置Main相机参数
        var mainCameraObj = new GameObject("MainCamera");
        var mainCamera = mainCameraObj.AddComponent<Camera>();
        mainCamera.clearFlags = CameraClearFlags.Depth;
        mainCamera.tag = "MainCamera";
        mainCamera.orthographic = true;
        mainCamera.orthographicSize = 5.4f;

        //设置虚拟相机参数（目前只有跟随相机）
        var brain = mainCameraObj.AddComponent<CinemachineBrain>();
        var vcam = new GameObject("vcam_PlayerRole").AddComponent<CinemachineVirtualCamera>();
        vcam.AddCinemachineComponent<CinemachineTransposer>();
        vcam.Follow = role.Transform;
    }

   


    private void InitLevel(int level)
    {

        PlayerManager.SpawnCharacter();
        //加载关卡：场景，地形，敌人
        var levelGobj = Object.Instantiate(Resources.Load<GameObject>($"Prefab/Level/{level}"));
      


        //换成addressable版本

       ////加载并生成血槽实例  尝试换成异步加载的形式
       //var hpBarGobj = Object.Instantiate(Resources.Load<GameObject>("Prefab/UI/HealthBar"));
       // hpBarGobj.transform.SetParent(UIManager.canvasTransform, false);

       // //为加载好的敌人设置血槽（这里应该只设置了单个吧？需要一个level管理器）（可能之后需要抽象成一个节点？）
       // var enemyGobj = EnemyManager.SpawnEnemy<BaseEnemy>(enemyPrefabName);
       // var enemy = GameObject.Find("enemy");
       // hpBarGobj.GetComponent<HealthBar>().SetOwner(enemy.transform);
    }

  
}
