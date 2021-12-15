using UnityEngine;
using UnityEngine.UI;


//包含所有 角色 和 地图 的选择画面
public class BtlModePanel : BasePanel
{
    public override void OnOpen()
    {
        var startGameButton = Find<Button>("PlayButton");
        startGameButton.onClick.AddListener(() =>
        {
            SAIGameManager.StartBtlGame<BTLGameModeController>();
            Close();//其实是关闭了面板的，但是相机一直不更新渲染图像(或者说一直没有删掉之前帧渲染过的图像)导致看上去好像没管，hierarchy上是显示关了的
            //好像是相机Depth Only的锅
        });
        //读取存档
        Find<Button>("DifficultyButton").onClick.AddListener(() => Debug.Log("难易度调节"));

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
