using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode1Controller  : GameController
{
    //需要序列化的敌人名（可能之后需要抽象成一个节点？）
    private string enemyPrefabName;

    public override void StartGame(int level)
    {
        Init(level);
    }

    private void Init(int level)
    {
        PlayerManager.SpawnCharacter();
        //加载关卡：场景，地形，敌人
        var levelGobj = Object.Instantiate(Resources.Load<GameObject>($"Prefab/Level/{level}"));
      

       //加载并生成血槽实例  尝试换成异步加载的形式
       var hpBarGobj = Object.Instantiate(Resources.Load<GameObject>("Prefab/UI/HealthBar"));
        hpBarGobj.transform.SetParent(UIManager.canvasTransform, false);

        //为加载好的敌人设置血槽（这里应该只设置了单个吧？需要一个level管理器）（可能之后需要抽象成一个节点？）
        var enemyGobj = EnemyManager.SpawnEnemy<BaseEnemy>(enemyPrefabName);
        var enemy = GameObject.Find("enemy");
        hpBarGobj.GetComponent<HealthBar>().SetOwner(enemy.transform);
    }

    public override void ExitGame()
    {
        throw new System.NotImplementedException();
    }
}
