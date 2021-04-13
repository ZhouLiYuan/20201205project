using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScene:BaseScene
{
    /// <summary>
    /// 场景名称
    /// </summary>
    readonly string sceneName = "Game";

    public override void OnEnter()
    {
        //如果没有激活的scene的Name叫 Game 的，就加载一个
        if (SceneManager.GetActiveScene().name != sceneName)
        {
            SceneManager.LoadScene(sceneName);
            SceneManager.sceneLoaded += SceneLoaded;
        }
    }

    //参数列表 必须与事件sceneLoaded一致
    private void SceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        SceneManager.SetActiveScene(scene);

        Debug.Log($"{sceneName}场景加载完毕！");
    }

    public override void OnExit()
    {
        //最后解除订阅，防止重复订阅
        SceneManager.sceneLoaded -= SceneLoaded;
    }
}
