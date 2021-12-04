using Role;
using Role.SelectableRole;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

//！！！！！！！！原则上不能让其他模块依赖（只是运行时的调试工具）！！！！！！！！！！
//目前前只能从静态类中获得实例
//或者直接API在场景中找（容易出现null报错）
//主要是负责在游戏进行（也就是Update的时候动态设置数据）

//SerializedMonoBehaviour不建议做成预制体
[Serializable, TypeInfoBox("运行时数值调试器")]
public class RuntimeOdinInspector : SerializedMonoBehaviour
{
    private void Start()//先手动把必须初始化的部分初始化一下
    {
        SynchronizePlayerRole();
        SynchronizeSceneObj();
        SynchronizeEnemyNameDic();
        SynchronizeNPCNameDic();
    }

    [TabGroup("Player")]
    [GUIColor(1, .5f, 0, 1)]//橙色
    [ShowInInspector, Searchable]
    [PropertyOrder(0)]//属性顺序
    [InlineButton("SynchronizePlayerRole", "运行时同步设置")]
    private PlayerRole playerRole;//只会在InSpector中序列化引用中的public字段（属性无法序列化）
    private void SynchronizePlayerRole() { playerRole = PlayerManager.m_Role; }


    [TabGroup("Player")][ShowInInspector, EnableGUI][PropertyOrder(1)]
    [InlineButton("SetPlayerHp", "运行时设置")]
    [GUIColor(0.5f, 1, 0, 1)]//绿色
    [ProgressBar(0, 100)]//以进图条形式显示变量
    private int hp;
    private void SetPlayerHp() { playerRole.HP = hp; }



    //[TabGroup("Player")][ShowInInspector]
    //[PropertyRange(0, 15)]//好像是属性值区间
    //另一种修改属性方式
    //[InlineButton("SetMoveSpeed")]
    //private float moveSpeed;
    //private void SetMoveSpeed() { playerRole.moveSpeed = moveSpeed; }


    //[ShowInInspector][TabGroup("Player")]
    //这样修改属性需要两步：1.输入设置 jumpSpeed值 2右键菜单调用设置属性方法
    //[CustomContextMenu("设置跳跃速度属性", "SetJumpSpeed")]//CustomContextMenu("右键时菜单中显示方法名", "脚本中需要调用的方法命")
    //private float jumpSpeed;
    //private void SetJumpSpeed() { playerRole.jumpSpeed = jumpSpeed; }


    //-----------------------场景原件信息----------------------------------

    //字典（运行时）
    [TabGroup("SceneObj")]
    [ShowInInspector, Searchable]
    [InlineButton("SynchronizeSceneObj", "运行时同步设置")]
    [TableColumnWidth(50, Resizable = false)]
    private List<GameObject> hookableEntities ;
    private void SynchronizeSceneObj()
    {
        hookableEntities = SceneObjManager.HookableEntities;
        interactableEntities = SceneObjManager.InteractableEntities;
    }
  
    [TabGroup("SceneObj")]
    [ShowInInspector, Searchable]
    [TableList, TableColumnWidth(20, Resizable = false)]//设置表的列宽，Resizable限制拖拽宽度 三道杠处可手动切换显示模式
    private List<GameObject> interactableEntities;
   
    
    //-----------------------敌人信息----------------------------------

    //字典（运行时）
    [TabGroup("Enemy")] [ShowInInspector, Searchable]
    [InlineButton("SynchronizeEnemyNameDic", "运行时同步设置")]
    [DictionaryDrawerSettings]//字典所在的脚本必须继承Odin序列化后的SerializedMonoBehaviour
    private Dictionary<string, Enemy> EnemyNameDic;
    private void SynchronizeEnemyNameDic() { EnemyNameDic = EnemyManager.nameDic; }

    //运行时动态追加敌人(每次一个)
    [TabGroup("Enemy")]
    public EnemyInfo info;
    [TabGroup("Enemy")]
    [ShowInInspector, Searchable]
    [InlineButton("SpawnEnemies", "追加生成")]
    private List<Enemy> extraEnemies = new List<Enemy>();
    private void SpawnEnemies() 
    {
        if (info == null) return;
        var enemy = LevelManager.ConfigureEnemy(info);
        extraEnemies.Add(enemy);
        var hookableGobj = enemy.Find<GameObject>("Hookable");
        SceneObjManager.HookableEntities.Add(hookableGobj);
    }


    //-----------------------NPC信息----------------------------------
    [TabGroup("NPC")]
    [ShowInInspector, Searchable]
    [InlineButton("SynchronizeNPCNameDic", "运行时同步设置")]
    [DictionaryDrawerSettings]
    private Dictionary<string, NPC> NPCNameDic;
    private void SynchronizeNPCNameDic() { NPCNameDic = NPCManager.nameDic; }
}






