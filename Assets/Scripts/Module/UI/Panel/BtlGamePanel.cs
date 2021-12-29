using Role;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游玩时的视窗
/// </summary>
public class BtlGamePanel : BasePanel
{
    public GameObject player1_HealthBarGobj;
    public GameObject player1_CircleGaugeGobj;
    public GameObject player2_HealthBarGobj;
    public GameObject player2_CircleGaugeGobj;

    private HealthBar pl1_healthBar;
    private HealthBar pl2_healthBar;
    private AwakeningGauge pl1_circleGauge;
    private AwakeningGauge pl2_circleGauge;

    List<HealthBar> healthBars = new List<HealthBar>();
    List<AwakeningGauge> awakeningGauges = new List<AwakeningGauge>();

    public override void OnOpen()
    {
        Init();
    }

    private void Init()
    {
        //好像是因为下面的命令是在LockUI Gobj生成前就调用到了(调试结果发现lockUI始终是null没有被赋值，并且调试程序没有继续执行)
        //lockUI = Find<GameObject>("LockUI");
        //lockUI.SetActive(false);

        player1_HealthBarGobj = Find<GameObject>("Player1_HealthBar");
        player1_CircleGaugeGobj = Find<GameObject>("Player1_CircleGauge");

        player2_HealthBarGobj = Find<GameObject>("Player2_HealthBar");
        player2_CircleGaugeGobj = Find<GameObject>("Player2_CircleGauge");

        pl1_healthBar = new HealthBar(player1_HealthBarGobj);
        pl1_healthBar.SetOwner(BtlCharacterManager.p1_Role);
        healthBars.Add(pl1_healthBar);
        pl2_healthBar = new HealthBar(player2_HealthBarGobj);
        pl2_healthBar.SetOwner(BtlCharacterManager.p2_Role);
        healthBars.Add(pl2_healthBar);

        pl1_circleGauge = new AwakeningGauge(player1_CircleGaugeGobj);
        pl1_circleGauge.SetOwner(BtlCharacterManager.p1_Role);
        awakeningGauges.Add(pl1_circleGauge);
        pl2_circleGauge = new AwakeningGauge(player2_CircleGaugeGobj);
        pl2_circleGauge.SetOwner(BtlCharacterManager.p2_Role);
        awakeningGauges.Add(pl2_circleGauge);

        foreach (var healthBar in healthBars)
        {healthBar.GameObject.SetActive(true);}

        foreach (var gauge in awakeningGauges)
        { gauge.GameObject.SetActive(true);}
    }


    public override void OnUpdate(float deltaTime)
    {
        foreach (var healthBar in healthBars)
        {healthBar.OnUpdate(deltaTime);}

        foreach (var gauge in awakeningGauges)
        {gauge.OnUpdate(deltaTime);}
    }



}
