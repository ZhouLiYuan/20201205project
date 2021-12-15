using Role.SelectableRole;
using UnityEngine;

public class BTLGameModeController : GameController
{
    private BtlGamePanel gamePanel;

    private PlayerRole player_1;
    private PlayerRole player_2;

    private GameObject ScreenCenterGobj;//相机跟随用

    Updater m_updater;

    public override void StartGame(int P1_characterID = 0, int P2_characterID = 1)
    {
        //player_1 = PlayerManager.SpawnPlayer1();
        //PlayerInput p1_playerInput = new PlayerInput();
        //p1_playerInput.InitInput();
        //player_1.BindInput(p1_playerInput);

        ////防止相机计算和血槽空引用暂时把2P也给赋值上
        //player_2 = PlayerManager.SpawnPlayer1();
        //PlayerInput p2_playerInput = new PlayerInput();//创建玩家 控制器
        //p2_playerInput.InitInput();
        //player_2.BindInput(p1_playerInput);

        //初始化关卡场景和相机
        CameraManager.InitCamera();
        ScreenCenterGobj = new GameObject("ScreenCenter");
        CameraManager.SetAdvModeAim(ScreenCenterGobj.transform);

        BTLBackGroundManager.SpawnStage();
        BTLBackGroundManager.Init();


        m_updater = Updater.AddUpdater();
        m_updater.AddUpdateFunction(DamageSystem.OnUpdate);
        m_updater.AddUpdateFunction(BTLBackGroundManager.OnUpdate);
        ////目前这两行会报错
        //m_updater.AddUpdateFunction(CalculateScreenCenterPos);//实时计算相机跟随目标
        //gamePanel = UIManager.OpenPanel<BtlGamePanel>();


        var OdinInspector = new GameObject();
        OdinInspector.name = "RuntimeOdinInspector";
        OdinInspector.AddComponent<RuntimeOdinInspector>();
    }

    private void CalculateScreenCenterPos() 
    {
        var screenCenterPos = (player_1.Transform.position + player_2.Transform.position) * 0.5f;
        ScreenCenterGobj.transform.position = screenCenterPos;
    }

    public override void ExitGame()
    {
   
    }
}
