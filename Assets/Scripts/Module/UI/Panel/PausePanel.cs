using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 暂停菜单
/// </summary>
public class PausePanel : Panel
{
    private GameObject questPanel;
    private GameObject optionPanel;
    /// <summary>
    /// 第二层级面板
    /// </summary>
    private GameObject secondaryPanel;
   
    /// <summary>
    /// level用来标记所在层级
    /// </summary>
    private int level;

    //按钮字段
    private Button mapButton;
    private Button skillButton;
    private Button questButton;
    private Button optionButton;

    private Button mainQuestButton;
    private Button secondaryQuestButton;

    private Text detailText;


    /// <summary>
    /// 第一层级面板
    /// </summary>
    enum TopBarSelect { None,Quest,Option };
    TopBarSelect topBarSelect = TopBarSelect.None;
    //TopBarSelect 的 Quest元素 对应着 整个QuestSelect（枚举的树状层级）
    /// <summary>
    /// 第二层级面板
    /// </summary>
    enum QuestSelect { None,Main,Secondary}
    QuestSelect questSelect = QuestSelect.None;


    public override void OnOpen()
    {
        InitUI();
        Register();
        //当前处在第一层级
        level = 1;
    }

    public override void OnUpdate(float deltaTime)
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            //如果是在第二层的面板
            if (level == 2) 
            {
                //退回第一层面板（TopBarSelect），重置枚举状态
                level = 1;
                topBarSelect = TopBarSelect.None;
                //关闭二层面板
                secondaryPanel.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 初始化储存 按钮 信息的字段
    /// </summary>
    private void InitUI() 
    {
        mapButton = m_transform.Find("TopBarPanel/MapButton").GetComponent<Button>();
        skillButton = m_transform.Find("TopBarPanel/SkillButton").GetComponent<Button>();
        questButton = m_transform.Find("TopBarPanel/QuestButton").GetComponent<Button>();
        optionButton = m_transform.Find("TopBarPanel/OptionButton").GetComponent<Button>();

        mainQuestButton = m_transform.Find("QuestPanel/MainQuestButton").GetComponent<Button>();
        secondaryQuestButton = m_transform.Find("QuestPanel/SecondaryQuestButton").GetComponent<Button>();

        detailText = m_transform.Find("QuestPanel/DetailPanel/Text").GetComponent<Text>();

        questPanel = m_transform.Find("QuestPanel").gameObject;
        optionPanel = m_transform.Find("OptionPanel").gameObject;
    }

    /// <summary>
    /// 每个 回调方法 都必须单独注册 到 各个按钮 OnClick事件
    /// </summary>
    private void Register()
    {
        //mapButton.onClick.AddListener();
        questButton.onClick.AddListener(OnQuest);
        optionButton.onClick.AddListener(OnOption);

        mainQuestButton.onClick.AddListener(OnMainQuest);
        secondaryQuestButton.onClick.AddListener(OnSecondaryQuest);
    }


    #region 一级菜单(回调方法)
    private void OnQuest() 
    {
        //当前层级变为2
        level = 2;
        topBarSelect = TopBarSelect.Quest;
        secondaryPanel = questPanel;
        questPanel.SetActive(true);
    }
    private void OnOption()
    {
        level = 2;
        topBarSelect = TopBarSelect.Option;
        secondaryPanel = optionPanel;
        optionPanel.SetActive(true);
    }
    #endregion

    #region 二级菜单(回调方法)
    private void OnMainQuest() 
    {
        questSelect = QuestSelect.Main;
        detailText.text = "主线";
    }
    private void OnSecondaryQuest()
    {
        questSelect = QuestSelect.Secondary;
        detailText.text = "支线";
    }

    #endregion


}