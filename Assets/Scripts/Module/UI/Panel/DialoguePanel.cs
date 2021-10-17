using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//布局参考洛克人X8  Enemy也算是NPC
public class DialoguePanel : BasePanel
{
    //临时工
    public NPCInteractablePanel temp;

    //表现层
    private GameObject targetDialogPanel;
    private GameObject playerDialogPanel;

    //UI组件
    private Text playerSideText;
    private Image playerSideHeadIcon;

    private Text targetSideText;
    private Image targetSideHeadIcon;

    //input数据（数组第一个string元素的index是0）
    private DialogueConfig[] dialogues;

    //逻辑
    enum State { standby, printLetterOneByOne, waitForConfirm }
    private State state = State.standby;

    private int dialogueIndex;//一句台词在台本中的index（行） 
    private int letterIndex;//字母在一句台词中的index
    private Text printSideText;//中转值
    private string sentenceWaitToPrint;//中转值


    private float elapsedTime = 0f;//经过的时间
    private float interval = 0.1f;//打印下一个字母的时间间隔（输出速度）


    private void Init()
    {
        targetDialogPanel = Find<GameObject>("TargetSide");
        playerDialogPanel = Find<GameObject>("PlayerSide");

        targetSideText = targetDialogPanel.transform.Find("Text").GetComponent<Text>();
        playerSideText = playerDialogPanel.transform.Find("Text").GetComponent<Text>();


        targetSideHeadIcon = targetDialogPanel.transform.Find("RoleImage").GetComponent<Image>();
        playerSideHeadIcon = playerDialogPanel.transform.Find("RoleImage").GetComponent<Image>();

        //Input数据
        dialogues = ResourcesLoader.LoadDialogue(StoryManager.currentEpisodeID);

        //逻辑
        PlayerManager.m_Role.playerInput.Interact.performed += OnPressInteract;//按下R键触发 本类体中OnInteract()
    }
    private void ResetDialogue()//重置相关逻辑参数，设置第一句对白
    {
        letterIndex = 0;
        elapsedTime = 0f;
        dialogueIndex = 0;
        MoveToNextContext(dialogues[dialogueIndex]);//先显示第一句
    }
    public override void OnOpen()
    {
        Init();
        ResetDialogue();
        Show();
    }



    //两种情况：1 执行下一句 2取消逐字直接显示全句
    public void OnPressInteract(UnityEngine.InputSystem.InputAction.CallbackContext callbackContext)
    {
        switch (state)
        {
            case State.waitForConfirm://等待按键操作
                if (dialogueIndex < dialogues.Length -1) //Length是从1开始计数的
                {
                    dialogueIndex++;
                    MoveToNextContext(dialogues[dialogueIndex]);
                }  
                else Close();
                break;
            case State.printLetterOneByOne://PrintWholeSentence
                printSideText.text = sentenceWaitToPrint;
                state = State.waitForConfirm;
                break;
            default:
                Debug.LogError("DialoguePanel的OnPressInteract方法出错！");
                break;
        }
    }

    //每句话打印完需要执行一次
    private void MoveToNextContext(DialogueConfig config)
    {
        if (config == null) return;

        sentenceWaitToPrint = config.Sentence;

        switch (config.SpeakSide)
        {
            case 0:
                playerSideHeadIcon.sprite = ResourcesLoader.LoadHeadIcon(config.RoleName, config.Emotion);
                printSideText = playerSideText;
                playerDialogPanel.SetActive(true);
          
                break;
            case 1:
                targetSideHeadIcon.sprite = ResourcesLoader.LoadHeadIcon(config.RoleName, config.Emotion);
                printSideText = targetSideText;
                targetDialogPanel.SetActive(true);
                break;
            default:
                break;
        }
        letterIndex = 0;//重置
        state = State.printLetterOneByOne;
    }

    public override void OnUpdate(float deltaTime)
    {
        if (state == State.printLetterOneByOne) PrintLetter(deltaTime);
        else return;
    }

    private void PrintLetter(float deltaTime)
    {
        elapsedTime += deltaTime;
        if (elapsedTime > interval)
        {
            elapsedTime = 0;
            letterIndex++;
            printSideText.text = sentenceWaitToPrint.Substring(0, letterIndex);
            if (letterIndex >= sentenceWaitToPrint.Length) state = State.waitForConfirm;
        }
    }


    public override void OnClose()
    {
        temp.Show();
        PlayerManager.m_Role.playerInput.Interact.performed -= OnPressInteract;
    }

}
