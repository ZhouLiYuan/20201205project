using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//注目这个命名空间
using UnityEngine.SceneManagement;

public class StartMenuScene : BaseScene
{
    /// <summary>
    /// 场景名称
    /// </summary>
    readonly string sceneName = "StartMenu";

   

    public override void OnEnter()
    {
        //如果没有激活的scene的Name叫 Start 的，就加载一个
        if (SceneManager.GetActiveScene().name != sceneName)
        {
           
            SceneManager.LoadScene(sceneName);
            SceneManager.sceneLoaded += SceneLoaded;
        }
    }

    //参数列表 必须与事件sceneLoaded一致
    private void SceneLoaded(Scene scene, LoadSceneMode loadSceneMode) 
    {
       
        UIManager.OpenPanelByCoroutine<StartMenuPanel>();

        SceneManager.SetActiveScene(scene);

        Debug.Log($"{sceneName}场景加载完毕！");
    }

public override void OnExit()
    {
        //最后解除订阅，防止重复订阅
        SceneManager.sceneLoaded -= SceneLoaded;
        //差一个关掉所有面板的方法
        //panelManager.PopAll();
    }

}
