using Role.SelectableRole;
using Role.SpineRole;
using UnityEngine;

public class BTLGameModeController : GameController
{
    private BtlGamePanel gamePanel;

    private PlayerRole_Spine player_1 => BtlCharacterManager.p1_Role;
    private PlayerRole_Spine player_2 => BtlCharacterManager.p2_Role;
    //private PlayerRole_Spine player_3 => BtlCharacterManager.p3_Role;
    //private PlayerRole_Spine player_4 => BtlCharacterManager.p4_Role;

    private GameObject ScreenCenterGobj;//相机跟随用

    Updater m_updater;

    public override void StartGame(int P1_characterID = 0, int P2_characterID = 1)
    {
        //初始化关卡场景和相机
        CameraManager.InitCamera();
        ScreenCenterGobj = new GameObject("ScreenCenter");
        CameraManager.SetAdvModeAim(ScreenCenterGobj.transform);

        //加载舞台
        BTLBackGroundManager.SpawnStage();
        BTLBackGroundManager.Init();
        var posConfigGobj = Object.Instantiate(ResourcesLoader.LoadBtlCharacterPosConfig());
        var p1_pos = posConfigGobj.transform.Find("P1");
        var p2_pos = posConfigGobj.transform.Find("P2");

        BtlCharacterManager.Init();//创建P1 P2
        player_1.Transform.SetParent(p1_pos,false);//配置出生位置
        player_2.Transform.SetParent(p2_pos,false);
        player_2.skeleton.ScaleX = -1f;//出生时应该面对player_1

        //绑定输入
        BtlPlayerInput p1_playerInput = new BtlPlayerInput();
        p1_playerInput.InitInput("P1");
        player_1.BindInput(p1_playerInput);

        //防止相机计算和血槽空引用暂时把2P也给赋值上
        BtlPlayerInput p2_playerInput = new BtlPlayerInput();//创建玩家 控制器
        p2_playerInput.InitInput("P2");//得提供另一套输入
        player_2.BindInput(p2_playerInput);


        //打开GamePanel（需要有PlayerRole，否则会空引用）
        gamePanel = UIManager.OpenPanel<BtlGamePanel>();//根据对战人数血槽数也要变更(参考任天堂大乱斗)

   

        m_updater = Updater.AddUpdater();
        m_updater.AddUpdateFunction(DamageSystem.OnUpdate);
        m_updater.AddUpdateFunction(BTLBackGroundManager.OnUpdate);
        m_updater.AddUpdateFunction(CalculateScreenCenterPos);//实时计算相机跟随目标



        var OdinInspector = new GameObject();
        OdinInspector.name = "RuntimeOdinInspector";
        OdinInspector.AddComponent<RuntimeOdinInspector>();
    }



    public override void ExitGame()
    {
   
    }


    private void CalculateScreenCenterPos()
    {
        var screenCenterPos = (player_1.Transform.position + player_2.Transform.position) * 0.5f;
        ScreenCenterGobj.transform.position = screenCenterPos + new Vector3(0,3,0);//暂时稍微偏上一些
    }
}
