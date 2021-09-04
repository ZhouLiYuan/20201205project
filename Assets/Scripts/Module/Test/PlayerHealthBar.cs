using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//目前只用于主角
[System.Serializable]
public class PlayerHealthBar : Entity
{
    public Slider m_slider;
    //颜色渐变功能
    public Gradient m_gradient;
    public Image m_fill;

    /// <summary>
    /// 角色最大血量，需要暴露在Inspector动态调节
    /// </summary>
    public float maxHealth;
    public float currentHealth;

    //初始化操作，建立逻辑层与表现层联系
    public PlayerHealthBar(GameObject obj):base(obj)
    {
        m_slider = GameObject.GetComponent<Slider>();
        m_fill = GameObject.GetComponentInChildren<Image>();

        maxHealth = PlayerManager.m_Role.health;
        currentHealth = maxHealth;
        m_slider.maxValue = maxHealth;
        m_slider.value = maxHealth;

        m_gradient = new Gradient();
        //gradient的数据条渐变是0到1，1就是最右边
        m_fill.color = m_gradient.Evaluate(1f);
    }

    /// <summary>
    /// 动态设置表现层
    /// </summary>
    /// <param name="health"></param>
    public void SetHealthBar(int health)
    {
        m_slider.value = health;
        //由于gradient的数据条渐变是0到1，所以需要把m_slider的数值归一化，换算成百分比
        m_fill.color = m_gradient.Evaluate(m_slider.normalizedValue);
    }

}
