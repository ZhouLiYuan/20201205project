using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 开始界面菜单（按钮 步骤三合一 整合版）
/// </summary>
public class StartMenuPanel : BasePanel
{
    /// <summary>
    /// StartMenu面板打开时调用的函数
    /// </summary>
    public override void OnOpen()
    {
        ////链式调用 整合 初始化组件 与 注册事件 以及匿名回调方法(lambda) 三个步骤
        ////UItool.GetOrAddComponentInChildren<Button>("").onClick.AddListener(() => {    } );
        //UItool.GetOrAddComponentInChildren<Button>("NewGameButton").onClick.AddListener(() => { GameRoot.Instance.MySceneManager.SetScene(new GameScene());  } );
        //UItool.GetOrAddComponentInChildren<Button>("LoadSaveDateButton").onClick.AddListener(() => { } );
        ////入栈
        //UItool.GetOrAddComponentInChildren<Button>("OptionButton").onClick.AddListener(() => { PanelManager.Push(new OptionPanel()); });
        ////出栈
        //UItool.GetOrAddComponentInChildren<Button>("QuitButton").onClick.AddListener(() => { PanelManager.Pop(); });

        var AdvModeButton = Find<Button>("AdvModeButton");
        // 默认选中开始按钮
        AdvModeButton.Select();
        AdvModeButton.onClick.AddListener(() =>
        {
            UIManager.OpenPanel<AdvModePanel>();
            Close();
        });
       

        Find<Button>("VersusModeButton").onClick.AddListener(() =>
        {
            UIManager.OpenPanel<BtlModePanel>();
            Close();
        } );

        //协程版本的打开Option面板
        Find<Button>("OptionButton").onClick.AddListener(() => UIManager.OpenPanel<OptionPanel>());
        //关闭面板
        Find<Button>("QuitButton").onClick.AddListener(()=> { Close(); });
    }
}
