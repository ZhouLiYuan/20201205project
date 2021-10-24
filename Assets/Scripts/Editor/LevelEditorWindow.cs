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

    private int levelID;
    private string levelName;
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
        GUILayout.Label("对应场景");

        //GUILayout.TextField创建一个可供用户编辑字符串的单行文本字段。
        levelID = int.Parse(GUILayout.TextField(levelID.ToString()));
        //尝试
        levelName = GUILayout.TextField(levelName);

        if (GUILayout.Button("保存关卡编辑信息"))
        {
            SaveLevelConfig();
        }

    }

    /// <summary>
    /// 记录场景中的配置(编辑信息)
    /// </summary>
    private void SaveLevelConfig()
    {
        //关卡信息
        LevelConfig levelConfig = ScriptableObject.CreateInstance<LevelConfig>();
        //LevelData levelConfig = new LevelConfig();也可以
        //姑且认为ScriptableObject的实例用 ScriptableObject配套API创建比较好（就像AddComponent）

        //值类型赋值
        levelConfig.levelID = levelID;
        levelConfig.levelName = levelName;

        //引用类型赋值(平台,敌人)

        //levelConfig.EnemyInfos = GetEntityIInfosList<EnemyInfo>();
        //levelConfig.PlatformInfos = GetEntityIInfosList<PlatformInfo>();
        levelConfig.EnemyInfos = GetEntityIInfosList<EnemyInfo, EnemyInfoInspector>();
        //levelConfig.PlatformInfos = GetEntityIInfosList<PlatformInfo, InfoInspector>();

        CreateAsset(levelConfig, levelID);

    }

    ///// <summary>
    ///// 获得Scene中XXList下的每个子Gobj 创建EntityInfo实例并初始化（表现层和逻辑层的联系）
    ///// 需要用TEntityInfo对应类中 的XXInfoInspector成员 修改其他客制化成员
    ///// </summary>
    ///// <typeparam name="TEntityInfo"></typeparam>
    ///// <returns></returns>
    //private List<TEntityInfo> GetEntityIInfosList<TEntityInfo>() where TEntityInfo : EntityInfo, new()
    //{
    //    //String字符是从0开始算起
    //    //LastIndexOf("")好像是会区分string大小写的？
    //    string fullName = typeof(TEntityInfo).Name;
    //    int index = fullName.LastIndexOf("I");
    //    var typeName = fullName.Substring(0, index);
    //    var entityList = GameObject.Find($"{typeName}List");

    //    var EntityInfos = new List<TEntityInfo>();

    //    //transform.childCount来直接获取子物体数量
    //    for (int i = 0; i < entityList.transform.childCount; i++)
    //    {
    //        var entityGobj_transform = entityList.transform.GetChild(i);
    //        //继承自InfoInspector的子类应该也能被Get到（类似Colider2d基类）
    //        var InfoInspector = entityGobj_transform.GetComponent<InfoInspector>();
    //        EntityInfos.Add(new TEntityInfo { infoInspector = InfoInspector });
    //    }
    //    return EntityInfos;
    //}

    //双参数类型版本
    private List<TEntityInfo> GetEntityIInfosList<TEntityInfo, TInfoInspector>()
    where TEntityInfo : EntityInfo<TInfoInspector>, new()
    where TInfoInspector : InfoInspector
    {
        //String字符是从0开始算起
        //LastIndexOf("")好像是会区分string大小写的？
        string fullName = typeof(TEntityInfo).Name;
        int index = fullName.LastIndexOf("I");
        var typeName = fullName.Substring(0, index);
        var entityList = GameObject.Find($"{typeName}List");

        var EntityInfos = new List<TEntityInfo>();

        //transform.childCount来直接获取子物体数量
        for (int i = 0; i < entityList.transform.childCount; i++)
        {
            var entityGobj_transform = entityList.transform.GetChild(i);
            //继承自InfoInspector的子类能被Get到（类似Colider2d基类）
            //默认InfoInspector挂在TopNode上
            var infoInspector = entityGobj_transform.GetComponent<TInfoInspector>();
            var entityInfo = new TEntityInfo();
            entityInfo.transform = entityGobj_transform;
            entityInfo.infoInspector = infoInspector;
            EntityInfos.Add(entityInfo);
            //EntityInfos.Add(new TEntityInfo { transform = entityGobj_transform ,infoInspector = infoInspector});
        }
        return EntityInfos;
    }

    /// <summary>
    /// 在指定路径创建 关卡资源文件
    /// </summary>
    /// <param name="data"></param>
    /// <param name="levelAssetPath"></param>
    private void CreateAsset(Object data, int levelID)
    {
        levelAssetPath = $"Assets/AssetBundles_sai/Level/{levelID}.asset";
        //这里主要都是针对project下操作的 API
        if (!data) throw new System.Exception("保存失败，拿个空场景也好意思让我保存？！");
        //确保保存关卡的目录已被创建
        if (!File.Exists(levelAssetPath)) { Directory.CreateDirectory(Directory.GetParent(levelAssetPath).FullName); }

        //只能使用工程相对路径
        AssetDatabase.CreateAsset(data, levelAssetPath);
        AssetDatabase.SaveAssets();
        //刷新project中的资源
        AssetDatabase.Refresh();

        //功能详解https://docs.unity3d.com/cn/current/ScriptReference/EditorGUIUtility.PingObject.html
        EditorGUIUtility.PingObject(data);
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = data;
    }

}


//---------------------<旧代码>--------------------------------

////引用类型
//var enemyList = GameObject.Find("EnemyList");
//var platformList = GameObject.Find("PlatformList");

////关卡中的敌人信息
//levelConfig.EnemyInfos = new List<EnemyInfo>();
////transform.childCount来直接获取子物体数量
//for (int i = 0; i < enemyList.transform.childCount; i++)
//{
//    var enGobj_transform = enemyList.transform.GetChild(i);
//    var enemyInfoInspector = enGobj_transform.GetComponent<EnemyInfoInspector>();
//    //为Scene中EnemyList下的每个子Gobj创建EnemyInfo实例并初始化（表现层和逻辑层的联系）
//    levelConfig.EnemyInfos.Add(new EnemyInfo
//    {
//        ID = enemyInfoInspector.ID,
//        WeaponID = enemyInfoInspector.WeaponID
//    });
//}

////关卡中的平台信息
//levelConfig.PlatformInfos = new List<PlatformInfo>();

//for (int i = 0; i < platformList.transform.childCount; i++)
//{
//    var pfGobj_transform = platformList.transform.GetChild(i);
//    levelConfig.PlatformInfos.Add(new PlatformInfo { Name = pfGobj_transform.name });
//}