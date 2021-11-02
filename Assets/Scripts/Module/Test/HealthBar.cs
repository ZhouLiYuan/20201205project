using Role;
using UnityEngine;
using UnityEngine.UI;

//T是血槽owner类型
[System.Serializable]
public class HealthBar: Entity
{
    public Slider m_slider;
    public Image m_fill;

    RoleEntity  owner;
    //这里有int 到 float的类型转换
    public float maxHealth => owner.maxHP;
    public float currentHealth => owner.HP;

    //血槽表现层
    public HealthBar(GameObject obj):base(obj)
    {
        m_slider = GameObject.GetComponent<Slider>();
        m_fill = Find<Image>("Fill");
    }

    public void SetOwner(RoleEntity owner)
    {
        this.owner = owner;
        m_slider.maxValue = maxHealth;//在还owner没有被赋值的时候调用maxHealth属性get方法会报错
    }

    public void SetColor()
    {
        if (maxHealth / 2 <= currentHealth) { m_fill.color = Color.green; }
        else if (maxHealth / 4 < currentHealth && currentHealth < maxHealth / 2) { m_fill.color = Color.yellow; }
        else if (0 < currentHealth && currentHealth <= maxHealth / 4) { m_fill.color = Color.red; }
        else if (currentHealth == 0) { m_fill.transform.gameObject.SetActive(false); }
        else { Debug.LogError($"当前血量{currentHealth}，血槽设置错误"); }
    }

    private void FollowOwner() 
    {
        if (GameObject.name == "PlayerHealthBar" || GameObject.name == "BossHealthBar") return;//固有的都不动
        var offset = new Vector3(0, 0.8f, 0);//之后应该根据每个npc大小调整
        UIManager.SetInteractUIPosition(owner.GameObject, GameObject, offset);
    }

    public void OnUpdate(float deltaTime) 
    {
        m_slider.value = currentHealth;
        SetColor();
        FollowOwner();
    }
}

// public Gradient gradient;
////由于gradient的数据条渐变是0到1，所以需要把m_slider的数值归一化，换算成百分比
//m_fill.color = m_gradient.Evaluate(m_slider.normalizedValue);


