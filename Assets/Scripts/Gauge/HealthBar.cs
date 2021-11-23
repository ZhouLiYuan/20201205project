using Role;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class HealthBar: BaseGauge
{

    //这里有int 到 float的类型转换
    public override float MaxValue => owner.maxHP;
    public override float CurrentValue => owner.HP;

    //血槽表现层
    public HealthBar(GameObject obj):base(obj){}


    private void FollowOwner() 
    {
        if (GameObject.name == "PlayerHealthBar" || GameObject.name == "BossHealthBar") return;//固有的都不动
        var offset = new Vector3(0, 0.8f, 0);//之后应该根据每个npc大小调整
        UIManager.SetInteractUIPosition(owner.GameObject, GameObject, offset);
    }

    public override void OnUpdate(float deltaTime) 
    {
        base.OnUpdate(deltaTime);
        base.SetColor(Color.green, Color.yellow, Color.red,0.5f, 0.25f);
        FollowOwner();
    }
}


