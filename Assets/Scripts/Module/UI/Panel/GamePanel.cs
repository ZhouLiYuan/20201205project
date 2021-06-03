using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePanel : BasePanel
{
    public override string Path => "Assets/AssetBundles_sai/UI/Panel/GamePanel.prefab";
    private GameObject lockUI;

    public override void OnOpen()
    {
        Init();
    }

    private void Init()
    {
        lockUI = Find<GameObject>("LockUI");
        lockUI.SetActive(false);
    }

    public void LockHint(GameObject target)
    {
        lockUI.SetActive(target != null);
        if (target)
        {
            //Postion赋值过程：target =>MainCamera =>UICamera =>lockUI
            var screenPosition = Camera.main.WorldToScreenPoint(target.transform.position);
            lockUI.transform.position = UIManager.UICamera.ScreenToWorldPoint(screenPosition);
        }

    }

}
