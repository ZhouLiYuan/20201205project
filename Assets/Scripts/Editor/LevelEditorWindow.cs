using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

//System.IO提供基本文件和目录支持

//也可以用odin改进下
//Obj中 注意区分 表现层Gobj/Prefab/Asset 和 逻辑层 instance/data

/// <summary>
/// 记录场景中的配置，反序列化成一些抽象信息？（比如敌人的位置与ID）（关卡编辑器）
/// 加载关卡的时候，再通过这些信息序列化加载出资源？
/// </summary>
public class LevelEditorWindow : EditorWindow
{
    private int level;
    private string levelAssetPath;

    [MenuItem("关卡编辑/关卡编辑器")]
    private static void Open()
    {
        var window = GetWindow<LevelEditorWindow>();
        window.title = "关卡编辑器";
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("再试试看？？");
        if (GUILayout.Button("保存关卡编辑信息"))
        {
            SaveLevelConfig();
        }
        //GUILayout.TextField创建一个可供用户编辑字符串的单行文本字段。
        level = int.Parse(GUILayout.TextField(level.ToString()));

    }

    /// <summary>
    /// 记录场景中的配置(编辑信息)
    /// </summary>
    private void SaveLevelConfig()
    {
        var enemyList = GameObject.Find("EnemyList");
        var platformList = GameObject.Find("PlatformList");

        //关卡信息
        LevelData levelData = ScriptableObject.CreateInstance<LevelData>();
        //LevelData levelData = new LevelData();之后再看有什么区别？


        //关卡中的敌人信息
        levelData.EnemyInfos = new List<EnemyInfo>();
        //transform.childCount来直接获取子物体数量
        for (int i = 0; i < enemyList.transform.childCount; i++)
        {
            var enGobj_transform = enemyList.transform.GetChild(i);
            var enemyInfoInspector = enGobj_transform.GetComponent<EnemyInfoInspector>();
            //为Scene中EnemyList下的每个子Gobj创建EnemyInfo实例并初始化（表现层和逻辑层的联系）
            levelData.EnemyInfos.Add(new EnemyInfo { ID = enemyInfoInspector.ID, Position = enGobj_transform.position, WeaponID = enemyInfoInspector.WeaponID });
        }

        //关卡中的平台信息
        levelData.PlatformInfos = new List<PlatformInfo>();

        for (int i = 0; i < platformList.transform.childCount; i++)
        {
            var pfGobj_transform = platformList.transform.GetChild(i);
            levelData.PlatformInfos.Add(new PlatformInfo { ID = pfGobj_transform.name, Position = pfGobj_transform.position });
        }

        levelAssetPath = $"Assets/AssetBundles_sai/Level/{level}.asset";
        CreateAsset(levelData, levelAssetPath);

    }

    /// <summary>
    /// 在指定路径创建 关卡资源文件
    /// </summary>
    /// <param name="data"></param>
    /// <param name="path"></param>
    private void CreateAsset(Object data, string path)
    {
        //这里主要都是针对project下操作的 API
        if (!data) throw new System.Exception("保存失败，拿个空场景也好意思让我保存？！");
        //确保保存关卡的目录已被创建
        if (!File.Exists(path)) { Directory.CreateDirectory(Directory.GetParent(path).FullName); }

        //只能使用工程相对路径
        AssetDatabase.CreateAsset(data, path);
        AssetDatabase.SaveAssets();
        //刷新project中的资源
        AssetDatabase.Refresh();

        //功能详解https://docs.unity3d.com/cn/current/ScriptReference/EditorGUIUtility.PingObject.html
        EditorGUIUtility.PingObject(data);
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = data;
    }

}
