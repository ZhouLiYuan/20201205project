using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 用继承自mono的类 来作为游戏运行入口
/// </summary>
public class GameEntrance : MonoBehaviour
{
    

    void Start()
    {
        UIManager.Open<PausePanel>();
    }

 
    void Update()
    {
        UIManager.Update();
    }
}
