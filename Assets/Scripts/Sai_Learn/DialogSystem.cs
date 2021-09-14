using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogSystem
{
    [Header("UI组件")]
    public Text m_textContent;
    public Image m_charaterFace;

    [Header("文本文件")]
    public TextAsset m_textFile;
    //Dialog的行数
    public int m_index;

    [Header("角色头像")]
    public Sprite m_player, m_enemy;

    [Header("文本输出速度")]
    public float m_inputSpeed;



    enum State { standby, print, waitForConfirm }
    private State state = State.standby;

    private DialogPanel dialogPanel;
    private int dialogueIndex;
    //这里的character和letter都是字母的意思
    private int characterIndex;
    private float elapsedTime = 0f;
    private float interval = 0.2f;
    private DialogueData[] dialogues;

    public void Init()
    {
        GlobalEvent.OnShowDialogue += ShowDialogue;
        dialogPanel = UIManager.Open<DialogPanel>();
        dialogPanel.Hide();
    }

    public void ShowDialogue(int storyId)
    {
        // 加载对话数据
        dialogues = ResourcesLoader.LoadDialogue(storyId);

        dialogueIndex = 0;
        characterIndex = 0;
        state = State.print;
        elapsedTime = 0f;

        dialogPanel.Show();
    }

    public void OnUpdate(float deltaTime)
    {
        switch (state)
        {
            // 按下确定键，继续对话或关闭
            case State.waitForConfirm:
                if (true)
                {
                    dialogPanel.Close();
                }
                break;
            // 输出对话，打字机效果
            case State.print:
                string dialogue = dialogues[dialogueIndex].Text;
                elapsedTime += deltaTime;
                if (elapsedTime > interval)
                {
                    elapsedTime -= interval;
                    characterIndex++;
                    dialogPanel.SetText(dialogue.Substring(0, characterIndex));
                    if (characterIndex >= dialogue.Length) state = State.waitForConfirm;
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
