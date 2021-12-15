using UnityEngine;
using UnityEngine.UI;


public class AdvModePanel : BasePanel
{
    public override void OnOpen()
    {
        var startGameButton = Find<Button>("NewGameButton");
        startGameButton.onClick.AddListener(() =>
        {
            //0是对话系统测试 1是战斗测试
            SAIGameManager.StartAdvGame<ADVGameModeController>(0);
            Close();
        });
        //读取存档
        Find<Button>("LoadSaveDateButton").onClick.AddListener(() => Debug.Log("读取存档"));


        //关闭面板
        Find<Button>("StartMenuButton").onClick.AddListener(() => 
        { 
            Close();
            UIManager.OpenPanel<StartMenuPanel>();
        });

        // 默认选中开始按钮
        startGameButton.Select();
    }

}
