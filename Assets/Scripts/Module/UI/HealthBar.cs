using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 模块脚本 表现层和逻辑层的连接就是currenthealth变量
/// </summary>
public class HealthBar : MonoBehaviour
{
    public Slider m_slider;
    //颜色渐变功能
    public Gradient gradient;
    public Image m_fill;

    public int maxHealth;
    public int currentHealth;

    //这个感觉也可以交给构造函数来做吧,mono里怎么调用构造函数
    /// <summary>
    /// 初始化生命值为最大值，并设置表现层
    /// </summary>
    /// <param name="health"></param>
    public void SetMaxHealth()
    {
        currentHealth = maxHealth;
        m_slider.maxValue = maxHealth ;
        m_slider.value = maxHealth ;
        //gradient的数据条渐变是0到1，1就是最右边
        m_fill.color = gradient.Evaluate(1f);
    }

    public void SetHealthBar(int health)
    {
        m_slider.value = health;
        //由于gradient的数据条渐变是0到1，所以需要把m_slider的数值归一化，换算成百分比
        m_fill.color = gradient.Evaluate(m_slider.normalizedValue);
    }

   
}