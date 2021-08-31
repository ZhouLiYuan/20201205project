using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogSystem
{

    enum TypeState { standby,print,waitForConfirm}
    private TypeState m_typeState = TypeState.standby;

    private DialogPanel m_dialogPanel;
    private int dialogueIndex;
    //单个角色对话出现的对话的字符index
    private int characterIndex;
    private float elapsedTime = 0f;
    private float interval = 0.2f;
    private InteractableDialogueData[] dialogues;


    public void Init() 
    {
        GlobalEvent.OnShowDialogue += ShowDialogue;

        m_dialogPanel = UIManager.Open<DialogPanel>();
        m_dialogPanel.Hide();
    }

    /// <summary>
    /// 单独show一个角色的一句话
    /// </summary>
    /// <param name="storyId"></param>
    public void ShowDialogue(int storyId) 
    {
        //加载对话数据
        dialogues = ResourcesLoader.LoadDialogue(storyId);

        //为什么每次都要归0
        dialogueIndex = 0;
        characterIndex = 0;
        m_typeState = TypeState.print;
        elapsedTime = 0f;

        m_dialogPanel.Show();
    }
    
    //非协程实现逐字打印版本
    public void OnUpdate(float deltaTime) 
    {
        switch (m_typeState)
        {
            //这里应该是是可以把true换成 再次按下R键的判断?
            case TypeState.waitForConfirm:
                if (true)
                {
                    m_dialogPanel.Close();
                }
                break;
            //逐字打印效果
            case TypeState.print:
                //指定 赋值整个对话 的某一行
                string dialogue = dialogues[dialogueIndex].Text;

                elapsedTime += deltaTime;
                //当消逝时间大于间隔时间时才可以进入
                if (elapsedTime > interval)
                {
                    //没看懂这里面是什么原理？是为了自动重置elapsedTime？elapsedTime = 0放在循环末尾也可以起到同样效果？
                    elapsedTime -= interval;
                    characterIndex++;
                    //指定不断重新配置  m_textContent，从第0个字母开始 每循环一次多一个字
                    m_dialogPanel.SetText(dialogue.Substring(0,characterIndex));

                    //当打印到最后一个字符的时候切换状态（为什么不是全部打完(characterIndex >= dialogue.Length)的时候??）
                    if (characterIndex >= dialogue.Length - 1) m_typeState = TypeState.waitForConfirm;
                }
                break;
            default:
                break;

        }   
    }


    private void OnDestroy() 
    {
        GlobalEvent.OnShowDialogue -= ShowDialogue;
    }

}