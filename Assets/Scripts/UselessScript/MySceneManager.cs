using System.Collections;
using System.Collections.Generic;
using UnityEngine;


///// <summary>
///// 场景状态管理系统,（区别于 UnityEngine.SceneManagement命名空间下的SceneManager）
///// </summary>
//public class MySceneManager
//{
//    BaseScene currentScene;

//    /// <summary>
//    /// （退出旧场景）设置并进入 当前场景
//    /// </summary>
//    /// <param name="scene"></param>
//    public void SetScene(BaseScene scene) 
//    {
//        //如果场景不为空，说明之前的操作给baseScene赋过值，需要先退出旧场景
//        currentScene?.OnExit();

//        //设置并进入 当前场景
//        currentScene = scene;
//        currentScene?.OnEnter();
//    }
//}
