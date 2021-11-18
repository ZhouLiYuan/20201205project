using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 用继承自mono的类 来作为游戏运行入口
/// </summary>
public class GameEntrance : MonoBehaviour
{
    //IEnumerator Start()
    //{
    //    yield return UIManager.Init();
    //    yield return UIManager.OpenPanelByCoroutine<StartMenuPanel>();
    //}

    void Start() 
    {
        UIManager.OpenPanel<StartMenuPanel>();
    }
 
    void Update()
    {
        UIManager.OnUpdate();
    }
}







