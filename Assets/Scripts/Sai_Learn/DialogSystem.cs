﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour
{
    [Header("UI组件")]
    public Text textContent;
    public Image charaterFace;

    [Header("文本文件")]
    public TextAsset textFile;
    //Dialog的行数
    public int m_index;

    //数组第一个string元素的index是0
    List<string> m_textList = new List<string>();

    private void OnEnable()
    {
        textContent.text = m_textList[m_index];
        //Debug.Log($"第一句台词行号{m_index}");
        m_index++;
    }

    void Awake()
    {
        GetTextFromFile(textFile);
    }

    void Update()
    {
        //判断文本是否以及输出到最后一行，避免m_index无限叠加
        if (Input.GetKeyDown(KeyCode.R) && m_index == m_textList.Count)
        {
            //就取消panel自身的激活状态
            gameObject.SetActive(false);
            //重置行号（重播）
            m_index = 0;
            return;
        }
        //每按一次R，就逐行输出文本
        if (Input.GetKeyDown(KeyCode.R)) 
        {
            textContent.text = m_textList[m_index];
            m_index++;
        }    
    }



    void GetTextFromFile(TextAsset file) 
    {
        m_textList.Clear();
        m_index = 0;

        //'\n'是Split的其中一种指定 按 行 切割 的API
        // 以\n为分割符将文本分割为一个数组
        //text是TextAsset中的自带属性（功能：整个TextAsset中的text内容会被当做一个 string类型数据）
        var lineDate = file.text.Split('\n');

        foreach (var line in lineDate)
        {
            m_textList.Add(line);
        }
    }


}
