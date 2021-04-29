using System;
using UnityEngine;

/// <summary>
/// 等待面板加载完成
/// </summary>
/// <typeparam name="Tpanel"></typeparam>
public class WaitOpenPanel<Tpanel> : CustomYieldInstruction where Tpanel : BasePanel,new()
{
    public override bool keepWaiting => !IsCompleted;
    public bool IsCompleted { get; private set; } = false;

    /// <summary>
    /// 需要打开的面板
    /// </summary>
    public Tpanel m_panel;

    /// <summary>
    /// 协程结束时的回调
    /// </summary>
    public event Action<Tpanel> OnFinish;

    /// <summary>
    /// <see cref="Panel"/>打开后的回调
    /// </summary>
    public event Action<Tpanel> OnOpened;
   

    /// <summary>
    /// 等待面板打开
    /// 所需参数：
    /// 1 需要一个加载面板的协程  2 无返回值openPanel委托 3 ？？？
    /// </summary>
    /// <param name="loadPanelPrefabCoroutine"></param>
    /// <param name="openPanelAction"></param>
    /// <param name="onOpended"></param>
    public WaitOpenPanel(WaitLoadAsset<GameObject> loadPanelPrefabCoroutine, Action<GameObject> openPanelAction, Action<Tpanel> onOpended)
    {
        OnOpened += onOpended;
        loadPanelPrefabCoroutine.OnFinish += _ =>
        {

            openPanelAction?.Invoke(loadPanelPrefabCoroutine.m_asset);
            m_panel = new Tpanel();
            IsCompleted = true;
            OnOpened?.Invoke(m_panel);
            OnFinish?.Invoke(m_panel);
        };

    }
}
