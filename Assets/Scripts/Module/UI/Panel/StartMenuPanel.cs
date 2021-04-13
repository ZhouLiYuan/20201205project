using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 开始界面菜单（按钮 步骤三合一 整合版）
/// </summary>
public class StartMenuPanel : BasePanel
{
    static readonly string path = "Resources/Prefab/UI/Panel/StartMenuPanel";

    /// <summary>
    /// 创建UIInfo实例时，会调用其 有参构造函数（需要传入string类型 数据）
    /// </summary>
    public StartMenuPanel() : base(new UIInfo(path)) { }

    public override void OnOpen()
    {
        //链式调用 整合 初始化组件 与 注册事件 以及匿名回调方法(lambda) 三个步骤
        //UItool.GetOrAddComponentInChildren<Button>("").onClick.AddListener(() => {    } );
        UItool.GetOrAddComponentInChildren<Button>("NewGameButton").onClick.AddListener(() => { GameRoot.Instance.MySceneManager.SetScene(new GameScene());  } );
        UItool.GetOrAddComponentInChildren<Button>("LoadSaveDateButton").onClick.AddListener(() => { } );
        //入栈
        UItool.GetOrAddComponentInChildren<Button>("OptionButton").onClick.AddListener(() => { PanelManager.Push(new OptionPanel()); });
        //出栈
        UItool.GetOrAddComponentInChildren<Button>("QuitButton").onClick.AddListener(() => { PanelManager.Pop(); });
    }

}
