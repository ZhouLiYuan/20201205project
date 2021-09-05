using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class LevelManager
{
    /// <summary>
    /// 加载并生成 主角Gobj，并创建 主角脚本实例
    /// (根据AssetBundles_sai文件夹下prefab名)
    /// </summary>
    /// <returns></returns>
    public static void InitLevel(int level)
    {
        //加载关卡：场景，地形，敌人
        //var levelGobj = Object.Instantiate(Resources.Load<GameObject>($"Prefab/Level/{level}"));
        var prefab = Object.Instantiate(AssetModule.LoadAsset<GameObject>($"Level/{level}.prefab"));

        //要有System.Linq命名空间才能使用where关键字
        //获取场景中所有名字带有Hookable的Gobj
        HookableManager.Init(Object.FindObjectsOfType<GameObject>().Where(obj => obj.name.Contains("Hookable")));

        var levelConfig = ResourcesLoader.LoadLevelConfig(level);
        for (int i = 0; i < levelConfig.EnemyInfos.Count; i++)
        {
            var enemyInfo = levelConfig.EnemyInfos[i];
            var enemyConfig = ResourcesLoader.LoadEnemyConfigByID(enemyInfo.ID);
            var enemy = EnemyManager.SpawnEnemy<BaseEnemy>(enemyConfig.AssetPath);
            enemy.InitProperties(enemyConfig);
            var weaponConfig = ResourcesLoader.LoadWeaponConfigByID(enemyInfo.ID);
            enemy.EquipWeapon(weaponConfig);
            //WeaponManager.InitWeapon<BaseWeapon>();
        }
        int enemyConfigID = 0;
        int weaponConfigID = 0;




        //换成addressable版本

        ////加载并生成血槽实例  尝试换成异步加载的形式
        //var hpBarGobj = Object.Instantiate(Resources.Load<GameObject>("Prefab/UI/HealthBar"));
        // hpBarGobj.transform.SetParent(UIManager.canvasTransform, false);

        // //为加载好的敌人设置血槽（这里应该只设置了单个吧？需要一个level管理器）（可能之后需要抽象成一个节点？）
        // var enemyGobj = EnemyManager.SpawnEnemy<BaseEnemy>(enemyPrefabName);
        // var enemy = GameObject.Find("enemy");
        // hpBarGobj.GetComponent<HealthBar>().SetOwner(enemy.transform);
    }

}



