using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController
{
    private Character m_character;

    public void StartGame(int level)
    {
        Init(level);
    }

    private void Init(int level)
    {
        //加载主角(这里还应该只是加载了脚本)
        m_character = Object.FindObjectOfType<Character>();
        //加载关卡：场景，地形，敌人
        var levelGobj = Object.Instantiate(Resources.Load<GameObject>($"Prefab/Level/{level}"));


        //加载并生成血槽实例  尝试换成异步加载的形式
        var hpBarGobj = Object.Instantiate(Resources.Load<GameObject>("Prefab/UI/HealthBar"));
        hpBarGobj.transform.SetParent(UIManager.canvasTransform, false);

        //为加载好的敌人设置血槽（这里应该只设置了单个吧？）
        var enemy = GameObject.Find("enemy");
        hpBarGobj.GetComponent<HealthBar>().SetOwner(enemy.transform);
    }

}
