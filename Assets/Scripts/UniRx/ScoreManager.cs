using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;

using TMPro;
using UniRx;//计时器API也很好用

//模仿鬼泣不同阶段评分(村上设置在吃到药丸时才会进入fever)
//村上喜欢把带返回值的方法当成一个只读属性来用
public enum ComboLabelType 
{
    C,B,A
}


//计分系统（以后可以搞个字典记录每关连击数据，给个评价）升级为评价系统evaluationSystem
public class ScoreManager : SingletonMonoBehaviour<ScoreManager>
{
    //表现层演出部分

    public int CurrentCombo => m_totalCombo.Value;
    [SerializeField] Image currentComboLabel;
    [SerializeField] TextMeshProUGUI currentComboText;

    //目前 村上的方法 全部都需要在Inspector中手动赋值
    //自处建议 1脚本写死 2写成ScriptableObject 在对应文件里配置好
    [SerializeField] ComboViewConfig combo_C;
    [SerializeField] ComboViewConfig combo_B;
    [SerializeField] ComboViewConfig combo_A;

    [SerializeField] ComboBorder[] m_comboBorders;//在Inspector中自行决定分界个数


    //逻辑部分
    const string ScoreSaveKey = "";


    public bool IsHighScore => (m_totalScore.Value >= LoadScore());//是否刷新最高分

    //UniRx自定义相应属性
    IntReactiveProperty m_totalScore = new IntReactiveProperty(0);//initialValue最初值指的是什么的最初值？
    public IReadOnlyReactiveProperty<int> TotalScore => m_totalScore;//Score变更通知
    IntReactiveProperty m_totalCombo = new IntReactiveProperty(0);
    public IReadOnlyReactiveProperty<int> TotalCombo => m_totalCombo;



    //当前连招奖励倍率 (比如连续消灭十个敌人时，倍率为2，原本一个敌人奖励为100，现在变成200)
    public float GetCurrentMagnification()
    {
        var element = m_comboBorders
            .Where(x =>
            {//x是ComboBorder类型的参数
                if (m_comboBorders.LastOrDefault() == x)
                { return x.BorderMax <= CurrentCombo; }
                return x.BorderMin <= CurrentCombo &&
                CurrentCombo <= x.BorderMax;//为什么中途是返回布尔值？又是怎么确认 x是ComboBorder类型的参数的？
            }).FirstOrDefault();//返回第一个元素(或集合为空时返回默认值) 

        return element != null ? element.Magnification : 1.0f;
        //x?y:z x为 true 返回y |false 返回z
    }

    //根据玩家当前连招评价信息改变 评价的UI演出
    public void ChangeComboLabel(ComboLabelType type)
    {
        switch (type)
        {
            case ComboLabelType.C:
                ConfigConboLabel(combo_C);
                break;
            case ComboLabelType.B:
                ConfigConboLabel(combo_B);
                break;
            case ComboLabelType.A:
                ConfigConboLabel(combo_A);
                break;
            default:
                break;
        }
    }
    private void ConfigConboLabel(ComboViewConfig comboConfig)
    {
        currentComboLabel.sprite = comboConfig.ComboSprite;
        currentComboLabel.color = comboConfig.ComboColor;
        currentComboText.color = comboConfig.ComboColor;
    }






    #region Score相关方法:Add Save Load Reset Delete

    //参数 击败当前敌人 获得的固有分数
    //处理：最终加分=固有分数 x 当前连击倍率
    public int AddScore(int score)
    {
        AddCombo();
        m_totalScore.Value += (int)(score * GetCurrentMagnification());//增加总分
        return (int)(score * GetCurrentMagnification());
    }

    public void ScoreSave()
    {
        if (m_totalScore.Value < LoadScore()) return;
        PlayerPrefs.SetInt(ScoreSaveKey, m_totalScore.Value);
        PlayerPrefs.Save();//直接存在磁盘中
    }
    //获取 玩家以往最高分
    public int LoadScore()
    {
        //PlayerPrefs持久化储存，声明周期是多久，似乎Unity被关闭后重新打开依然存在
        return PlayerPrefs.GetInt(ScoreSaveKey, 0);//貌似因为是储存最高分，所以只需要一个Key一个Value，
    }
    //清空（重置当前得分）
    public void ResetScore()
    {
        m_totalScore.Value = 0;
    }
    //删除保存的得分记录
    public void DeleteScore()
    {
        PlayerPrefs.DeleteKey(ScoreSaveKey);//删除键和匹配的值
    }

    #endregion



    #region Combo相关方法:Add Clear
    //感觉这种写法有点脱裤子放屁, 目的是从方法名 提高可读性？
    public void AddCombo() { m_totalCombo.Value++; }

    public void ResetCombo() { m_totalCombo.Value = 0; }

    #endregion
}


[Serializable]
public class ComboViewConfig
{
    [SerializeField] Sprite m_comboSprite;
    [SerializeField] Color m_comboColor;

    public Sprite ComboSprite => m_comboSprite;
    public Color ComboColor => m_comboColor;
}

/// <summary>
/// 连招评价变化边界值（也是倍率奖励的变化边界）
/// </summary>
[Serializable]
public class ComboBorder
{
    [SerializeField] int m_borderMin = 0;
    [SerializeField] int m_borderMax = 0;
    [SerializeField] float m_magnification = 1.0f;//borderMin~borderMax区间范围内的奖励倍率

    //确保了只能通过Inspector配置变量值
    public int BorderMin => m_borderMin;
    public int BorderMax => m_borderMax;
    public float Magnification => m_magnification;
}
