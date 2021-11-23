using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AwakeningGauge : BaseGauge
{
    //这里有int 到 float的类型转换
    public override float MaxValue => owner.maxHP;
    public override float CurrentValue =>  owner.maxHP - owner.HP ;//暂定和血槽值反着来

    public AwakeningGauge(GameObject obj) : base(obj) { }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
        base.SetColor(Color.red, Color.yellow, Color.blue, 0.9f, 0.4f);
    }
}
