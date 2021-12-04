using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using Sirenix.OdinInspector.Editor.Modules;
using Role.SelectableRole;
using System;
using Role;
//官方教程https://odininspector.com/blog/custom-inspector-tutorial


//public enum GameState
//{
//    startScene,
//    gamePlay,
//    paused,
//    complete
//}


//SerializedMonoBehaviour不建议做成预制体
[Serializable, TypeInfoBox("预览与Asset配置")]
public class EditorModeOdinInspector : SerializedMonoBehaviour
{
    //[BoxGroup("Game State Info")]
    //[EnumToggleButtons]
    //[OnValueChanged("StateChange")]
    //[ShowInInspector]
    //public GameState gameState;

    [FilePath]
    [InfoBox("可以用来读project asset地址")]//显示在Inspector中的注释(用于属性)，\n换行
    public string AssetPath;

    [FolderPath]
    public string AssetFolderPath;

    [AssetSelector]
    public Material mat;

    [PreviewField]//预览
    public Sprite sprite;




    [BoxGroup("关卡")]
    [PropertySpace]//在属性间加空格
    [HideLabel]//隐藏成员名称
    [LabelText("需要加载的关卡序号")]//可以配合[HideLabel]修改显示在面板中成员名称
    [InlineButton("LoadLevelConfig", "从项目中加载资源")]//常用于设置，读取，重置
    [InlineButton("InitSynchroField", "初始化同步字段")]//!!!!!!不知为啥，有了这一行特性，即便对应方法注释掉，每次保存这个脚本的时候场景中总会生成一个EnemyList Gobj（上一行相同的特性就不会引发这种问题。。和特性的顺序也无关。。）
    public int levelIndex = 0;
    private void LoadLevelConfig() { levelConfig = ResourcesLoader.LoadLevelConfig(levelIndex); }
    private void InitSynchroField()
    {
        //非同步版本
        enemyInfos = CopyEnemyInfo(levelConfig.EnemyInfos);

        //同步版本
        //enemyInfos = levelConfig.EnemyInfos;
        npcInfos = levelConfig.NPCInfos;
        platformInfos = levelConfig.PlatformInfos;
        checkPointInfos = levelConfig.CheckPointInfos;
    }

    //[BoxGroup("关卡")]
    //[ShowInInspector][ReadOnly]
    //[DetailedInfoBox("关卡信息", "平台，敌人，NPC，检查点信息，使用时请先加载与初始化")]
    //[InlineEditor(InlineEditorModes.GUIOnly)]//只能用于修饰 继承自ScriptableObject的引用类型字段
    private LevelConfig levelConfig;


    [ShowInInspector, Searchable]
    [ListDrawerSettings(ShowIndexLabels = true)]
    private List<PlatformInfo> platformInfos;

    [ShowInInspector, Searchable]
    private List<NPCInfo> npcInfos;

    [ShowInInspector, Searchable]
    [InlineEditor(InlineEditorModes.GUIOnly)]
    private List<CheckPointInfo> checkPointInfos = new List<CheckPointInfo>();

    //-----------------------敌人信息----------------------------------
    [ShowInInspector]
    [TabGroup("Enemy")]
    [PropertyTooltip("数据来源于project的levelConfig")]//鼠标悬停在字段名时的提示
    [InlineButton("OverrideEnemyInfos", "覆盖Project数据")]
    [ListDrawerSettings(ShowIndexLabels = true, DraggableItems = false)]//DraggableItems默认是true，即可通过在面板拖拽改变list中元素的顺序
    [ValueDropdown("enemyInfos")]//必须与修饰的List成员名一致，可用于折叠 引用类型元素的成员信息
    [Searchable]//通过关键字搜索成员(适用于一些集合 字段)   使用条件: 1访问级别为public 2可Set
    private List<EnemyInfo> enemyInfos;
    private void OverrideEnemyInfos() { levelConfig.EnemyInfos = CopyEnemyInfo(enemyInfos); }


    //-----------------------一些工具-----------------------------------
    //有没有复制引用的API？
    //现在只能通过创建新缓存实例 拆箱参数的值类型成员挨个 装入缓存实例中的方式达到 “复制引用”的效果
    private List<EnemyInfo> CopyEnemyInfo(List<EnemyInfo> enemyInfos)
    {
        var cachedInfos = new List<EnemyInfo>();
        foreach (var enemyInfo in enemyInfos)
        {
            var cachedInfo = new EnemyInfo();
            cachedInfo.Position = enemyInfo.Position;
            cachedInfo.AssetID = enemyInfo.AssetID;
            cachedInfo.WeaponID = enemyInfo.WeaponID;
            cachedInfos.Add(cachedInfo);
        }
        return cachedInfos;
    }


    //[TableMatrix(SquareCells = false)]//需继承SerializedMonoBehaviour，可以将二维数组转化为表的显示形式
    //public int[,] MapData = {
    //    { 1,1,1,0} ,
    //    { 1,1,0,1},
    //    { 1,0,1,1},
    //    { 0,1,1,1}
    //};
}



