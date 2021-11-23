using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//分割线素材
//------------------------------<TopBarPanel>-----------------------------------
//--------------------------------<MapPanel>------------------------------------
//--------------------------------<Skill面板>---------------------------------------
//-------------------------------<任务QuestPanel>--------------------------------
//--------------------------------<OptionPanel>-----------------------------------


/// <summary>
/// 暂停菜单(按钮拆分 三步骤 详细版)
/// </summary>
public class PausePanel : BasePanel
{
    //static readonly string path = "Resources/Prefab/UI/Panel/PausePanel";
    //改为addressable的Name
    public override string Path => "Panel/PausePanel.prefab";

    ///// <summary>
    ///// 创建UIInfo实例时，会调用其 有参构造函数（需要传入string类型 数据）
    ///// </summary>
    //public PausePanel() : base(new UIInfo(path)) { }



    private GameObject mapPanel;
    private GameObject skillPanel;
    private GameObject questPanel;
    private GameObject optionPanel;


    //层级
    /// <summary>
    /// 第二层级面板
    /// </summary>
    private GameObject secondaryPanel;
    /// <summary>
    /// level用来标记所在层级
    /// </summary>
    private int level;

    //按钮字段
    //-------------------<TopBarPanel>----------------------
    private Button mapButton;
    private Button skillButton;
    private Button questButton;
    private Button optionButton;
    //---------------------------------<MapPanel>------------------------------------
    private Button filterButton;
    private Button markButton;
    private Button fastTravelButton;
    //--------------------------------<Skill面板>---------------------------------------
    private Button maskPowerButton;
    private Button abilitiesButton;

    //-----------------------------<任务QuestPanel>--------------------------------------
    private Button mainQuestButton;
    private Button secondaryQuestButton;
    //--------------------------------<OptionPanel>-----------------------------------------
    private Button difficultyButton;
    private Button audioButton;
    private Button videoButton;
    private Button controllerButton;
    private Button keyboardButton;

    //Quest面板任务详情
    private Text detailText;
    //Skill面板技能点数
    private Text abilityPointText;

    /// <summary>
    /// 第一层级面板
    /// </summary>
    enum TopBarSelect { None,Map,Skill,Quest,Option };
    TopBarSelect topBarSelect = TopBarSelect.None;

    //------------<二级（枚举的树状层级）>------------

    /// <summary>
    /// 第二层级面板MapPanel
    /// </summary>
    enum MapSelect { None, Filter, Mark, FastTravel }
    MapSelect mapSelect = MapSelect.None;

    /// <summary>
    /// 第二层级Skill面板
    /// </summary>
    enum SkillSelect { None, Abilities, MaskPower }
    SkillSelect skillSelect = SkillSelect.None;

    /// <summary>
    /// 第二层级面板Skill面板
    /// </summary>
    enum QuestSelect { None,Main,Secondary}
    QuestSelect questSelect = QuestSelect.None;

    /// <summary>
    /// 第二层级面板OptionPanel
    /// </summary>
    enum OptionSelect { None,Difficulty,Audio,Video, Controller, Keyboard }
    OptionSelect optionSelect = OptionSelect.None;



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
    public override void OnPause()
    {
        base.OnPause();
    }

    public override void OnResume()
    {
        base.OnResume();
    }
    public override void OnClose()
    {
        base.OnClose();
    }

    /// <summary>
    /// 初始化储存 按钮 信息的字段 ( m_transform是prefab topNode PausePanel的transform)
    /// </summary>
    private void InitUI() 
    {
        //这里有没有办法优化成一个for循环，一口气把所有按钮初始化

        //----------------------------------------------<TopBarPanel>------------------------------------------------------
        //Btn  Button = m_transform.Find("Panel/Button").GetComponent<Button>();
        mapButton = Transform.Find("TopBarPanel/MapButton").GetComponent<Button>();
        skillButton = Transform.Find("TopBarPanel/SkillButton").GetComponent<Button>();
        questButton = Transform.Find("TopBarPanel/QuestButton").GetComponent<Button>();
        optionButton = Transform.Find("TopBarPanel/OptionButton").GetComponent<Button>();

        //----------------------------------------------<MapPanel>------------------------------------------------------
        mapPanel = Transform.Find("MapPanel").gameObject;
        //Btn
        filterButton = Transform.Find("MapPanel/FilterButton").GetComponent<Button>();
        markButton = Transform.Find("MapPanel/MarkButton").GetComponent<Button>();
        fastTravelButton = Transform.Find("MapPanel/FastTravelButton").GetComponent<Button>();
        //----------------------------------------------<Skill面板>------------------------------------------------------
        skillPanel = Transform.Find("SkillPanel").gameObject;
        //Btn
        abilitiesButton = Transform.Find("SkillPanel/AbilitiesButton").GetComponent<Button>();
        maskPowerButton = Transform.Find("SkillPanel/MaskPowerButton").GetComponent<Button>();
        //Txt
        abilityPointText = Transform.Find("SkillPanel/AbilityPointText").GetComponent<Text>();

        //----------------------------------------------<任务QuestPanel>------------------------------------------------------
        questPanel = Transform.Find("QuestPanel").gameObject;
        //Btn
        mainQuestButton = Transform.Find("QuestPanel/MainQuestButton").GetComponent<Button>();
        secondaryQuestButton = Transform.Find("QuestPanel/SecondaryQuestButton").GetComponent<Button>();
        //Txt
        detailText = Transform.Find("QuestPanel/DetailPanel/Text").GetComponent<Text>();

        //----------------------------------------------<OptionPanel>------------------------------------------------------
        optionPanel = Transform.Find("OptionPanel").gameObject;
        //Btn
        difficultyButton = Transform.Find("OptionPanel/DifficultyButton").GetComponent<Button>();
        audioButton = Transform.Find("OptionPanel/AudioButton").GetComponent<Button>();
        videoButton = Transform.Find("OptionPanel/VideoButton").GetComponent<Button>();
        controllerButton = Transform.Find("OptionPanel/ControllerButton").GetComponent<Button>();
        keyboardButton = Transform.Find("OptionPanel/KeyboardButton").GetComponent<Button>();
    }

    /// <summary>
    /// 每个 回调方法 都必须单独注册 到 各个按钮 OnClick事件
    /// </summary>
    private void Register()
    {
        //----------------------------------------------<TopBarPanel>------------------------------------------------------
        mapButton.onClick.AddListener(OnMap);
        skillButton.onClick.AddListener(OnSkill);
        questButton.onClick.AddListener(OnQuest);
        optionButton.onClick.AddListener(OnOption);
        //----------------------------------------------<MapPanel>------------------------------------------------------
        filterButton.onClick.AddListener(OnFilter);
        markButton.onClick.AddListener(OnMark);
        fastTravelButton.onClick.AddListener(OnFastTravel);
        //--------------------------------<Skill面板>---------------------------------------
        abilitiesButton.onClick.AddListener(OnAbilities);
        maskPowerButton.onClick.AddListener(OnMaskPower);
        //-------------------------------<任务QuestPanel>--------------------------------
        mainQuestButton.onClick.AddListener(OnMainQuest);
        secondaryQuestButton.onClick.AddListener(OnSecondaryQuest);
        //--------------------------------<OptionPanel>-----------------------------------
        difficultyButton.onClick.AddListener(OnDifficulty);
       audioButton.onClick.AddListener(OnAudio);
        videoButton.onClick.AddListener(OnVideo);
       controllerButton.onClick.AddListener(OnController);
        keyboardButton.onClick.AddListener(OnKeyboard);
    }


    #region 一级菜单(回调方法)
    private void ResetSecondaryPanel() 
    {
        mapPanel.SetActive(false);
        skillPanel.SetActive(false);
        questPanel.SetActive(false);
        optionPanel.SetActive(false);
    }
    private void OnMap()
    {
        level = 2;
        ResetSecondaryPanel();
        topBarSelect = TopBarSelect.Map;
        secondaryPanel = mapPanel;
        mapPanel.SetActive(true);
    }
    private void OnSkill()
    {
        level = 2;
        ResetSecondaryPanel();
        topBarSelect = TopBarSelect.Skill;
        secondaryPanel = skillPanel;
        skillPanel.SetActive(true);
    }
    private void OnQuest() 
    {
        //当前层级变为2
        level = 2;
        ResetSecondaryPanel();
        topBarSelect = TopBarSelect.Quest;
        secondaryPanel = questPanel;
        questPanel.SetActive(true);
    }
    private void OnOption()
    {
        level = 2;
        ResetSecondaryPanel();
        topBarSelect = TopBarSelect.Option;
        secondaryPanel = optionPanel;
        optionPanel.SetActive(true);

        UIManager.OpenPanel<OptionPanel>();
    }
    #endregion

    #region 二级菜单(回调方法)

    //--------------------------------<MapPanel>------------------------------------
    private void OnFilter() { }
    private void OnMark() { }
    private void OnFastTravel() { }
    //--------------------------------<Skill面板>---------------------------------------
    private void OnAbilities() { }
    private void OnMaskPower() { }
    //-------------------------------<任务QuestPanel>--------------------------------
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
    //--------------------------------<OptionPanel>-----------------------------------
    private void OnDifficulty() { }
    private void OnAudio() { }
    private void OnVideo() { }
    private void OnController() { }
    private void OnKeyboard() { }

    #endregion


}