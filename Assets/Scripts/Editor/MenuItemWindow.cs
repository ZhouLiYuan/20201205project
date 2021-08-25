using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class MenuItemWindow
{
    [MenuItem("调试游戏/直接开始战斗场景")]
    private static void StartBattleScene() 
    {
        //bool 如果选择保存此类场景，则返回 true，如果按下 Cancel，则返回 false。
        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
        // 从Build Settings的Index来获得 场景路径
        var scenePath = SceneUtility.GetScenePathByBuildIndex(0);
        if (scenePath != null) EditorSceneManager.OpenScene(scenePath);
        //Editor进入调试播放模式
        EditorApplication.isPlaying = true;
    }

    [MenuItem("调试游戏/开始游戏")]
    private static void StartGame()
    {

    }
}
