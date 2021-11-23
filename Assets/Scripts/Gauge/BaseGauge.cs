using Role;
using UnityEngine;
using UnityEngine.UI;


public class BaseGauge : Entity
{
    public Slider m_slider;
    public Image m_fill;

    protected RoleEntity owner;
    //这里有int 到 float的类型转换
    public virtual float MaxValue { get; set; }
    public virtual float CurrentValue { get; set; }

    //血槽表现层
    public BaseGauge(GameObject obj) : base(obj)
    {
        m_slider = GameObject.GetComponent<Slider>();
        m_fill = Find<Image>("Fill");
    }

    public void SetOwner(RoleEntity owner)
    {
        this.owner = owner;
        m_slider.maxValue = MaxValue;//在还owner没有被赋值的时候调用maxHealth属性get方法会报错
    }

    /// <summary>
    /// ValuePos 是颜色的渐变等分点
    /// </summary>
    /// <param name="maxValueColor"></param>
    /// <param name="middleColor"></param>
    /// <param name="minValueColor"></param>
    /// <param name="bigValuePos"></param>
    /// <param name="lessValuePos"></param>
    public virtual void SetColor(Color maxValueColor,Color middleColor,Color minValueColor, float bigValuePos = 0.6f, float lessValuePos = 0.3f)
    {
        if (CurrentValue != 0) m_fill.transform.gameObject.SetActive(true);
        var middleBigValue = MaxValue * bigValuePos;
        var middleLessValue = MaxValue * lessValuePos;
        if (middleBigValue <= CurrentValue) { m_fill.color = maxValueColor; }
        else if (middleLessValue < CurrentValue && CurrentValue < middleBigValue) { m_fill.color = middleColor; }
        else if (0 < CurrentValue && CurrentValue <= middleLessValue) { m_fill.color = minValueColor; }
        else if (CurrentValue == 0) { m_fill.transform.gameObject.SetActive(false); }
        else { Debug.LogError($"{this.GetType()}当前数值{CurrentValue}，数据槽设置错误"); }
    }


    public virtual void OnUpdate(float deltaTime)
    {
        m_slider.value = CurrentValue;
        //SetColor(Color.white,Color.white,Color.white);//默认白色不变色
    }
}

// public Gradient gradient;
////由于gradient的数据条渐变是0到1，所以需要把m_slider的数值归一化，换算成百分比
//m_fill.color = m_gradient.Evaluate(m_slider.normalizedValue);
