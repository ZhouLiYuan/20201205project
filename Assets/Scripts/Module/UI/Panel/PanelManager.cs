using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager
{
    /// <summary>
    /// 储存UI面板的栈
    /// </summary>
    private Stack<BasePanel> stackPanel;
    private BasePanel panel;

    public PanelManager() 
    {
        stackPanel = new Stack<BasePanel>();
    }

    /// <summary>
    /// UI的入栈操作，此操作会显示一个面板
    /// </summary>
    /// <param name="nextPanel"></param>
    public void Push(BasePanel nextPanel) 
    {
        if (stackPanel.Count > 0)
        {
            panel = stackPanel.Peek();
            panel.OnPause();

            //递归为什么要用递归
            stackPanel.Push(nextPanel);
            // panelGo有是干嘛的
            GameObject panelGo = UIManager.GetSingleUI(nextPanel.UIInfo);
        }
    }

    public void Pop() 
    {
        //OnExit和OnClose合并
        if (stackPanel.Count > 0) stackPanel.Peek().OnClose();
        //为什么要分开if写两个？
        if (stackPanel.Count > 0) stackPanel.Peek().OnResume();
    }

    /// <summary>
    /// 执行所有面板的OnClose（）
    /// </summary>
    public void PopAll() 
    {
        //Pop()的返回值是BasePanel
        while (stackPanel.Count > 0)  { stackPanel.Pop().OnClose(); }
    }

}
