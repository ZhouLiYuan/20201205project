using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour
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

    //数组第一个string元素的index是0
    List<string> m_textList = new List<string>();
    private bool textOutputFinished = false;

    private void OnEnable()
    {
        textOutputFinished = true;
        //textContent.text = m_textList[m_index];
        ////Debug.Log($"第一句台词行号{m_index}");
        //m_index++;
        StartCoroutine(SetDialogUI());
    }

    void Awake()
    {
        GetTextFromFile(m_textFile);
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

    void Update()
    {
        ActiveDialog();
    }

    private void ActiveDialog() 
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
        if (Input.GetKeyDown(KeyCode.R)&&textOutputFinished)
        {
            
            //textContent.text = m_textList[m_index];
            //m_index++;
            StartCoroutine(SetDialogUI());
        }
    }

    /// <summary>
    /// 逐字输出台词
    /// </summary>
    /// <returns></returns>
    IEnumerator SetDialogUI() 
    {
        textOutputFinished = false;
        //换行时对话框内text重置
        m_textContent.text = "";

        //根据文本切换角色头像(若某一行文本全是 人名 则换头像并且前进一行)
        switch (m_textList[m_index]) 
        {
            //执行不进来
            case "煉獄":
                m_charaterFace.sprite = m_player;
                Debug.Log("能执行到switch语句但无法执行到case中");
                m_index++;
                break;
            case "あかざ":
                m_charaterFace.sprite = m_enemy;
                Debug.Log("能执行到switch语句但无法执行到case中");
                m_index++;
                break;
        }

        for (int i = 0; i < m_textList[m_index].Length; i++) 
        {
            //m_textList[m_index][i] 第m_index行的第i个字符 (按m_inputSpeed速度挨个+字符)
            m_textContent.text += m_textList[m_index][i];
            yield return new WaitForSeconds(m_inputSpeed);
        }
        //增加行数
        m_index++;

        textOutputFinished = true;
    }
    

}
