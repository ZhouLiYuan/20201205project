using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游玩时的视窗
/// </summary>
public class GamePanel : BasePanel
{
    public override string Path => "Panel/GamePanel.prefab";
    private GameObject lockUI;
    public GameObject healthBar;

    public override void OnOpen()
    {
        Init();
    }

    private void Init() 
    {
        //好像是因为下面的命令是在LockUI Gobj生成前就调用到了(调试结果发现lockUI始终是null没有被赋值，并且调试程序没有继续执行)
        lockUI = Find<GameObject>("LockUI");
        lockUI.SetActive(false);

        healthBar = Find<GameObject>("HealthBar");
        healthBar.SetActive(true);
    }


    /// <summary>
    /// 将目标位置转换成UICamera中的位置 并设置 激活LockUI的position为目标位置
    /// </summary>
    /// <param name="target"></param>
    public void SetLockHint(GameObject target)
    {
        UIManager.SetInteractUI(target, lockUI);
    }

}
