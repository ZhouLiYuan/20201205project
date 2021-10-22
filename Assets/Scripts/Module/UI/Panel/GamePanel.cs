using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游玩时的视窗
/// </summary>
public class GamePanel : BasePanel
{
    
    private GameObject lockUI;

    public GameObject playerHealthBarGobj;
    public GameObject bossHealthBarGobj;

    private HealthBar pl_healthBar;
    public static HealthBar boss_healthBar;//暂时默认全局只有一个boss

    //string为角色名称
    public static Dictionary<string, HealthBar> HealthBarNameDic = new Dictionary<string, HealthBar>();


    public override void OnOpen()
    {
        Init();
    }

    private void Init() 
    {
        //好像是因为下面的命令是在LockUI Gobj生成前就调用到了(调试结果发现lockUI始终是null没有被赋值，并且调试程序没有继续执行)
        lockUI = Find<GameObject>("LockUI");
        lockUI.SetActive(false);

        playerHealthBarGobj = Find<GameObject>("PlayerHealthBar");
        playerHealthBarGobj.SetActive(true);
        bossHealthBarGobj = Find<GameObject>("BossHealthBar");
        bossHealthBarGobj.SetActive(false);//等boss登场

        pl_healthBar = new HealthBar(playerHealthBarGobj);
        pl_healthBar.SetOwner(PlayerManager.m_Role);
        HealthBarNameDic[PlayerManager.m_Role.UniqueName] = pl_healthBar;
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
    }


    /// <summary>
    /// 将目标位置转换成UICamera中的位置 并设置 激活LockUI的position为目标位置
    /// </summary>
    /// <param name="target"></param>
    public void SetLockHint(GameObject target)
    {
        UIManager.SetInteractUIPosition(target, lockUI);
    }

}
