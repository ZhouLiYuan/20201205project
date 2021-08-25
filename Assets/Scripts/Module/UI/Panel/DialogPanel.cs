using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogPanel : BasePanel
{


    //文本输出速度(需要暴露的可调节值)
    public float m_inputWaitTime = 0.2f;
    private float currentTime;

    public override string Path => "Panel/DialogPanel.prefab";

    
    private GameObject DialogText;
    private GameObject HeadIcon;

    //文本资源
    private TextAsset m_textFile;

    //UI组件
    private Text m_textContent;
    private Image m_characterFace;

    //Dialog的行数
    private int m_index;

    //对话角色头像
   private Sprite m_playerSprite, m_enemySprite;

    //数组第一个string元素的index是0
    List<string> m_textList = new List<string>();

    /// <summary>
    /// 下一句打印前判断上一句是否输出完成
    /// </summary>
    private bool textOutputFinished = false;
    /// <summary>
    /// 跳过逐字打印
    /// </summary>
    private bool cancelTyping = false;

    private Coroutine OnTypingCoroutine;



    private event Action<string> OnDialogueCharacterIndexChanged;


    public override void OnOpen()
    {
        InitSomething();
    }

    private void InitSomething()
    {
        //m_gameObject.SetActive(false);
        //表现层逻辑层关联
        DialogText = Find<GameObject>("Text");
        HeadIcon = Find<GameObject>("Image");

        //这里只是Get到了，那以后怎么set呢？
        m_textContent = DialogText.GetComponent<Text>();
        m_characterFace = HeadIcon.GetComponent<Image>();

        ////这样可以从project中找到prefab赋值到变量m_textFile吗？
        //m_textFile = AssetModule.LoadAsset<TextAsset>("Text/DialogAsset.txt");
        //GetTextFromFile(m_textFile);
        //textOutputFinished = true;

        ////只是load没有实例化的话下面的case能顺利赋值吗？
        //m_playerSprite = Resources.Load<Sprite>("Sprites/Box_character");
        //m_enemySprite = Resources.Load<Sprite>("Sprites/Box_enemy");
    }

    void GetTextFromFile(TextAsset file)
    {
        m_textList.Clear();
        m_index = 0;

        //'\n'是Split的其中一种指定 按 行 切割 的API
        var lineDate = file.text.Split(new char[] { '\n', '\r' });


        foreach (var line in lineDate)
        {
            m_textList.Add(line);
        }
    }

    /// <summary>
    /// 之后吧R键变为InputSystem版本
    /// </summary>
    private void ActiveDialog()
    {
        //判断文本是否以及输出到最后一行，避免m_index无限叠加
        if (Input.GetKeyDown(KeyCode.R) && m_index == m_textList.Count)
        {
            //就取消panel自身的激活状态
            m_gameObject.SetActive(false);
            //重置行号（重播）
            m_index = 0;
            return;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            //当上一句输出完成 并且 没有 取消逐字输出时
            if (textOutputFinished && !cancelTyping)
            {
                SetDialogUI();
            }
            //当前句还未输出完成的时候，再次按下R键就会进入else if的判断
            else if (!textOutputFinished)
            {
                //把trigger true false互换的一种写法
                cancelTyping = !cancelTyping;
            }
        }
    }

    /// <summary>
    /// 逐字输出台词
    /// </summary>
    /// <returns></returns>
    void SetDialogUI()
    {
        textOutputFinished = false;
        //换行时对话框内text重置
        m_textContent.text = "";

        //根据文本切换角色头像(若某一行文本全是 人名 则换头像并且前进一行)
        switch (m_textList[m_index])
        {

            case "煉獄":
                m_characterFace.sprite = m_playerSprite;
                m_index++;
                break;
            case "あかざ":
                m_characterFace.sprite = m_enemySprite;
                m_index++;
                break;
            default:
                break;
        }

        int letter = 0;
        //不取消的情况下 逐字打印
        while (!cancelTyping && letter < m_textList[m_index].Length)
        {
            ///等待时间
            if (currentTime < m_inputWaitTime)
            {
                currentTime += Time.deltaTime;
            }
            else 
            {
                m_textContent.text += m_textList[m_index][letter];
                currentTime = 0;
                letter++;
            }
            
            //for (int i = 0; i < m_inputWaitTime; i++)
            //{
            //   
            //}
        }


        //直接赋值整行string
        m_textContent.text = m_textList[m_index];

        cancelTyping = false;
        textOutputFinished = true;
        //进入下一行
        m_index++;
    }

    public void SetText(string text)
    {
        m_textContent.text = text;
    }
}
