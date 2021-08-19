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

    /// <summary>
    /// 在下一句打印输出前判断上一句是否输出完成
    /// </summary>
    private bool textOutputFinished = false;
    /// <summary>
    /// 取消逐字输出(或者理解为 跳过逐字打印)
    /// </summary>
    private bool cancelTyping = false;

    private Coroutine OnTypingCoroutine;



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
        //var lineDate = file.text.Split('\n');
        var lineDate = file.text.Split(new char[] { '\n', '\r' });
       

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

            //终止协程写在这里合适吗？
            StopCoroutine(OnTypingCoroutine);


            return;
        }
        //每按一次R，就逐行输出文本
        //if (Input.GetKeyDown(KeyCode.R)&&textOutputFinished)
        //{
        //    //textContent.text = m_textList[m_index];
        //    //m_index++;
        //    StartCoroutine(SetDialogUI());
        //}

        if (Input.GetKeyDown(KeyCode.R)) 
        {
            //当上一句输出完成 并且 没有 取消逐字输出时
            if (textOutputFinished && !cancelTyping)
            {
               OnTypingCoroutine = StartCoroutine(SetDialogUI());
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
    IEnumerator SetDialogUI() 
    {
        textOutputFinished = false;
        //换行时对话框内text重置
        m_textContent.text = "";

        if (m_textList[m_index].Contains("煉獄")) 
        { }

        Debug.Log($"{m_textList[m_index]}11111");
        Debug.Log($"{m_textList[m_index]}"== "煉獄");
        //根据文本切换角色头像(若某一行文本全是 人名 则换头像并且前进一行)
        switch (m_textList[m_index]) 
        {
            
            case "煉獄":
                m_charaterFace.sprite = m_player;
                m_index++;
                break;
            case "あかざ":
                m_charaterFace.sprite = m_enemy;
                m_index++;
                break;
            default:
                break;
        }
        

        //for (int i = 0; i < m_textList[m_index].Length; i++) 
        //{
        //    //m_textList[m_index][i] 第m_index行的第i个字符 (按m_inputSpeed速度挨个+字符)
        //    m_textContent.text += m_textList[m_index][i];
        //    yield return new WaitForSeconds(m_inputSpeed);
        //}
        //for循环只能做计数循环，while循环可以自定义循环条件(当while的条件为 纯计数条件时两者可以互换)

        int letter = 0;
        //不取消的情况下才调用协程方法
        while (!cancelTyping && letter < m_textList[m_index].Length)
        {
            m_textContent.text += m_textList[m_index][letter];
            letter++;
            yield return new WaitForSeconds(m_inputSpeed);
        }


        //直接赋值整行string
        m_textContent.text = m_textList[m_index];

        cancelTyping = false;
        textOutputFinished = true;
        //进入下一行
        m_index++;
    }
    

}
