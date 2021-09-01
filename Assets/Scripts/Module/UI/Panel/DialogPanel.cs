using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Panel一般只是负责储存一些面板成员的信息(或者赋值/或者初始化一些成员)
public class DialogPanel : BasePanel
{

    //文本输出速度(需要暴露的可调节值)
    public float m_inputWaitTime = 0.2f;

    public override string Path => "Panel/DialogPanel.prefab";

    
    private GameObject DialogText;
    private GameObject HeadIcon;


    //UI组件
    private Text m_textContent;
    private Image m_characterFace;

    //对话角色头像
    private Sprite m_playerSprite, m_enemySprite;


    //可以看看这个委托是用来干嘛的？
    private event Action<string> OnDialogueCharacterIndexChanged;
  


    public override void OnOpen()
    {
        Init();
    }

    private void Init()
    {
        m_gameObject.SetActive(false);
        //表现层逻辑层关联
        DialogText = Find<GameObject>("DialogText");
        HeadIcon = Find<GameObject>("HeadIcon");

        //Get之后是可以直接通过为引用变量赋值来set对应的Component的
        m_textContent = DialogText.GetComponent<Text>();
        m_characterFace = HeadIcon.GetComponent<Image>();

        ////从project中找到prefab赋值到变量m_textFile
        //m_textFile = AssetModule.LoadAsset<TextAsset>("Text/DialogAsset.txt");
        //GetTextFromFile(m_textFile);
        //textOutputFinished = true;

        ////只load没有实例化的话下面的case也能顺利赋值
        //m_playerSprite = Resources.Load<Sprite>("Sprites/Box_character");
        //m_enemySprite = Resources.Load<Sprite>("Sprites/Box_enemy");
    }

    public void SetText(string text)
    {
        m_textContent.text = text;
    }
}

   