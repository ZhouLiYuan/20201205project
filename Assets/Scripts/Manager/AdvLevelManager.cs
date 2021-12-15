using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Role;


//尝试mono单例
//Level无model尝试
//加载关卡：场景，地形，敌人
public class AdvLevelManager
{
    public static ADVLevelConfig currentLevel;
    //目前为止已经打通的关卡
    public static Dictionary<string, ADVLevelConfig> nameDic = new Dictionary<string, ADVLevelConfig>();
    public static Dictionary<int, ADVLevelConfig> IDDic = new Dictionary<int, ADVLevelConfig>();

    /// <summary>
    /// 加载并生成 主角Gobj，并创建 主角脚本实例
    /// (根据AssetBundles_sai文件夹下prefab名)
    /// </summary>
    /// <returns></returns>
    public static void InitLevel(int level)
    {
        InitByPrefab(level);
        InitByConfiguration(level);

        //要有System.Linq命名空间才能使用Where关键字
        //获取场景中所有名字带有Hookable的Gobj
        ADVSceneGobjManager.InitHookableEntities(Object.FindObjectsOfType<GameObject>().Where(obj => obj.tag.Contains("Hookable")));
        //获取场景中所有Layer为Interact的Gobj
        ADVSceneGobjManager.InitInteractableEntities(Object.FindObjectsOfType<GameObject>().Where(obj => obj.layer == LayerMask.NameToLayer("Interact")));
    }

    //关卡地形，敌人都放在一个prefab加载
    private static void InitByPrefab(int level)
    {
        var prefab = Object.Instantiate(AssetModule.LoadAsset<GameObject>($"Level/{level}.prefab"));
    }

   public static void InitByConfiguration(int level)
    {
        //加载 场景配置
        var levelConfig = ResourcesLoader.LoadLevelConfig(level);

        for (int i = 0; i < levelConfig.EnemyInfos.Count; i++)
        {
            var enemyInfo = levelConfig.EnemyInfos[i];
            ConfigureEnemy(enemyInfo);
        }

        var npc_config = ResourcesLoader.LoadConfigByID<NPCConfig>(0);
        var npc = NPCManager.SpawnInstance(npc_config.AssetName, new Vector3(3, 2, 0));
        npc.InitProperties(npc_config);
        //var npc2 = NPCManager.SpawnInstance(npc_config.AssetName, new Vector3(-3, 2, 0));
        //npc2.InitProperties(npc_config);
    }


   public static Enemy ConfigureEnemy(EnemyInfo enemyInfo)
    {
        //根据enemyInfo加载详细的配置表
        var en_config = ResourcesLoader.LoadEnemyConfigByID(enemyInfo.AssetID);
        var weapon_config = ResourcesLoader.LoadWeaponConfigByID(enemyInfo.WeaponID);

        var enemy = EnemyManager.SpawnInstance(en_config.AssetName, enemyInfo.Position);

        enemy.InitProperties(en_config);
        UIManager.SpawnHealthBar(enemy);//HP属性初始化后HealthBar才能拿到配置后的初始值
        enemy.EquipWeapon(weapon_config);
        return enemy;
    }



    public static void ChangeLevel(int level) 
    {
        //需要销毁现有场景的信息 再加载新的Level
    }

    public static T GetInstanceByName<T>(string name) where T : ADVLevelConfig
    {
        if (nameDic.TryGetValue(name, out var t))
        {
            return t as T ;
        }
        return null;
    }

}



////加载并生成血槽实例  换成addressable版本加载的形式
//var hpBarGobj = Object.Instantiate(Resources.Load<GameObject>("Prefab/UI/HealthBar"));
// hpBarGobj.transform.SetParent(UIManager.canvasTransform, false);
// hpBarGobj.GetComponent<HealthBar>().SetOwner(enemy.transform);