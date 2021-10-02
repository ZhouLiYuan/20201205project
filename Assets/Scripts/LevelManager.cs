using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//加载关卡：场景，地形，敌人
public static class LevelManager
{

    /// <summary>
    /// 加载并生成 主角Gobj，并创建 主角脚本实例
    /// (根据AssetBundles_sai文件夹下prefab名)
    /// </summary>
    /// <returns></returns>
    public static void InitLevel(int level)
    {
        InitByPrefab(level);
        InitByConfiguration(level);

        //要有System.Linq命名空间才能使用where关键字
        //获取场景中所有名字带有Hookable的Gobj
        SceneObjManager.InitHookableEntities(Object.FindObjectsOfType<GameObject>().Where(obj => obj.name.Contains("Hookable")));
        SceneObjManager.InitInteractableEntities(Object.FindObjectsOfType<GameObject>().Where(obj => obj.name.Contains("Interactable")));
    }

    //关卡地形，敌人都放在一个prefab加载
    private static void InitByPrefab(int level)
    {
        //var levelGobj = Object.Instantiate(Resources.Load<GameObject>($"Prefab/Level/{level}"));
        var prefab = Object.Instantiate(AssetModule.LoadAsset<GameObject>($"Level/{level}.prefab"));
    }

   public static void InitByConfiguration(int level)
    {
        //加载 场景配置
        var levelConfig = ResourcesLoader.LoadLevelConfig(level);

        for (int i = 0; i < levelConfig.EnemyInfos.Count; i++)
        {
            var enemyInfo = levelConfig.EnemyInfos[i];

            //根据enemyInfo加载详细的配置表
            var en_config = ResourcesLoader.LoadEnemyConfigByID(enemyInfo.ID);
            var weapon_config = ResourcesLoader.LoadWeaponConfigByID(enemyInfo.WeaponID);
            //var en_config = ResourcesLoader.LoadEnemyConfigByName("持剑敌人");//"en_config_name"
            //var weapon_config = ResourcesLoader.LoadWeaponConfigByName("小牙签");//"weapon_config_name"
            
            var enemy = EnemyManager.SpawnEnemy<BaseEnemy>(en_config.AssetPath,enemyInfo.Position);
            //第二轮卡在这了，配置表也没问题，可能是状态机出了问题导致这边无法继续执行
            enemy.InitProperties(en_config);
            enemy.EquipWeapon(weapon_config);

            ////加载并生成血槽实例  换成addressable版本加载的形式
            //var hpBarGobj = Object.Instantiate(Resources.Load<GameObject>("Prefab/UI/HealthBar"));
            // hpBarGobj.transform.SetParent(UIManager.canvasTransform, false);
            // hpBarGobj.GetComponent<HealthBar>().SetOwner(enemy.transform);

        }
    }
}



