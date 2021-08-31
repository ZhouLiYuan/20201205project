using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    //数组第一个string元素的index是0
    List<string> m_textList = new List<string>();

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

        m_textContent = DialogText.GetComponent<Text>();
        m_characterFace = HeadIcon.GetComponent<Image>();

    }

    public void SetText(string text)
    {
        m_textContent.text = text;
    }
}
