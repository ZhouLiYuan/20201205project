using Role;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游玩时的视窗
/// </summary>
public class GamePanel : BasePanel
{
    
    //private GameObject lockUI;

    public GameObject playerHealthBarGobj;
    public GameObject playerCircleGaugeGobj;
    public GameObject bossHealthBarGobj;

    private HealthBar pl_healthBar;
    public static HealthBar boss_healthBar;//暂时默认全局只有一个boss

    private AwakeningGauge pl_circleGauge;

    //string为角色名称
    public static Dictionary<string, HealthBar> HealthBarNameDic = new Dictionary<string, HealthBar>();


    public override void OnOpen()
    {
        Init();
    }

    private void Init() 
    {
        //好像是因为下面的命令是在LockUI Gobj生成前就调用到了(调试结果发现lockUI始终是null没有被赋值，并且调试程序没有继续执行)
        //lockUI = Find<GameObject>("LockUI");
        //lockUI.SetActive(false);

        playerHealthBarGobj = Find<GameObject>("PlayerHealthBar");
        playerHealthBarGobj.SetActive(true);
        playerCircleGaugeGobj = Find<GameObject>("PlayerCircleGauge");
        playerCircleGaugeGobj.SetActive(true);
        bossHealthBarGobj = Find<GameObject>("BossHealthBar");
        bossHealthBarGobj.SetActive(false);//等boss登场

        pl_healthBar = new HealthBar(playerHealthBarGobj);
        pl_healthBar.SetOwner(PlayerManager.m_Role);
        HealthBarNameDic[PlayerManager.m_Role.UniqueName] = pl_healthBar;

        pl_circleGauge = new AwakeningGauge(playerCircleGaugeGobj);
        pl_circleGauge.SetOwner(PlayerManager.m_Role);//因为暂时只有Player有怒气槽 所以不用字典
    }

    public void SetBossHealthBar(RoleEntity owner) 
    {
        boss_healthBar = new HealthBar(bossHealthBarGobj);
        boss_healthBar.SetOwner(owner);
        HealthBarNameDic[owner.UniqueName] = boss_healthBar;
        bossHealthBarGobj.SetActive(true);
    }

    public  override void OnUpdate(float deltaTime)
    {
        foreach (var healthBar in HealthBarNameDic.Values) { healthBar.OnUpdate(deltaTime);}
        pl_circleGauge.OnUpdate(deltaTime);
    }



}
