using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//实现血条跟随帮助文档https://github.com/YuzikiRain/Learn/blob/master/Unity/Tips/%E5%AE%9E%E7%8E%B0%E8%A1%80%E6%9D%A1%E8%B7%9F%E9%9A%8F.md

/// <summary>
/// 模块脚本 表现层和逻辑层的耦合为currenthealth变量
/// </summary>
public class HealthBarOld : MonoBehaviour
{
    public Slider m_slider;

    //颜色渐变功能
    public Gradient gradient;
    public Image m_fill;

    public int maxHealth;
    public int currentHealth;

    /// <summary>
    /// 脚本挂载的 对象的 transform
    /// </summary>
    private Transform m_owner;

    /// <summary>
    /// 为脚本内m_owner字段传值
    /// </summary>
    /// <param name="owner"></param>
    public void SetOwner(Transform owner) { this.m_owner = owner; }

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

    /// <summary>
    /// 逻辑层映射到表现层
    /// </summary>
    /// <param name="health"></param>
    public void SetHealthBar(float health)
    {
        m_slider.value = health;
        //由于gradient的数据条渐变是0到1，所以需要把m_slider的数值归一化，换算成百分比
        m_fill.color = gradient.Evaluate(m_slider.normalizedValue);
    }

    private void Update()
    {
        if (!m_owner) return;

        var m_height = 0f;
        //Canvas设置为 Screen Space - Camera 的方法
        Vector3 viewPos = Camera.main.WorldToScreenPoint(m_owner.position + new Vector3(0f, m_height, 0f));

        //根据比率自动缩放 横轴纵轴？意思是相对长宽？
        var m_scaler = FindObjectOfType<CanvasScaler>();
        var widthRatio = m_scaler.referenceResolution.x / Camera.main.scaledPixelWidth;
        var heightRatio = m_scaler.referenceResolution.y / Camera.main.scaledPixelHeight;
        GetComponent<RectTransform>().anchoredPosition = new Vector2(viewPos.x * widthRatio, viewPos.y * heightRatio);
    }


}