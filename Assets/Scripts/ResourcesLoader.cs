using System;
using System.Collections.Generic;
using UnityEngine;

//封装路径
public static class ResourcesLoader
{
    private static EnemyConfig[] enemyConfigs;
    private static WeaponConfig[] weaponConfigs;

    public static DialogueData[] LoadDialogue(int storyId)
    {
        var json = AssetModule.LoadAsset<TextAsset>($"dialogueSample").text;
        var dialogueDatas = Newtonsoft.Json.JsonConvert.DeserializeObject<DialogueData[]>(json);
        List<DialogueData> results = new List<DialogueData>();
        for (int i = 0; i < dialogueDatas.Length; i++)
        {
            if (dialogueDatas[i].storyId != storyId) continue;
            results.Add(dialogueDatas[i]);
        }
        return results.ToArray();
    }

    public static GameObject LoadPrefab(string name)
    {
        return AssetModule.LoadAsset<GameObject>($"Prefab/{name}.prefab");
    }

    public static EnemyConfig LoadEnemyConfigByID(int id)
    {
        if (enemyConfigs == null)
        {
            var json = AssetModule.LoadAsset<TextAsset>($"Config/EnemyConfig.json").text;
            enemyConfigs = Newtonsoft.Json.JsonConvert.DeserializeObject<EnemyConfig[]>(json);
        }
        for (int i = 0; i < enemyConfigs.Length; i++)
        {
            if (enemyConfigs[i].ID == id) return enemyConfigs[i];
        }
        return null;
    }

    public static EnemyConfig LoadEnemyConfigByName(string name)
    {
        if (enemyConfigs == null)
        {
            var json = AssetModule.LoadAsset<TextAsset>($"Config/EnemyConfig.json").text;
             enemyConfigs = Newtonsoft.Json.JsonConvert.DeserializeObject<EnemyConfig[]>(json);
        }
        for (int i = 0; i < enemyConfigs.Length; i++)
        {
            if (enemyConfigs[i].Name == name) return enemyConfigs[i];
        }
        return null;
    }

    public static GameObject LoadEnemyPrefab(string name)
    {
        return AssetModule.LoadAsset<GameObject>($"Enemy/{name}.prefab");
    }



    public static GameObject LoadEffectPrefab(string name)
    {
        return AssetModule.LoadAsset<GameObject>($"Effect/{name}.prefab");
    }


    //用json配置
    public static WeaponConfig LoadWeaponConfigByID(int id)
    {
        if (weaponConfigs == null)
        {
            var json = AssetModule.LoadAsset<TextAsset>($"Config/WeaponConfig.json").text;
            weaponConfigs = Newtonsoft.Json.JsonConvert.DeserializeObject<WeaponConfig[]>(json);
        }
        for (int i = 0; i < weaponConfigs.Length; i++)
        {
            if (weaponConfigs[i].ID == id) return weaponConfigs[i];
        }
        return null;
    }

    public static WeaponConfig LoadWeaponConfigByName(string name)
    {
        if (weaponConfigs == null)
        {
            var json = AssetModule.LoadAsset<TextAsset>($"Config/WeaponConfig.json").text;
            weaponConfigs = Newtonsoft.Json.JsonConvert.DeserializeObject<WeaponConfig[]>(json);
        }
        for (int i = 0; i < weaponConfigs.Length; i++)
        {
            if (weaponConfigs[i].Name== name) return weaponConfigs[i];
        }
        return null;
    }

    public static GameObject LoadWeaponPrefab(string name)
    {
        return AssetModule.LoadAsset<GameObject>($"Weapon/{name}.prefab");
    }




    //用scriptable asset配置
    public static LevelConfig LoadLevelConfig(int level)
    {
        //project中的scriptable asset其实就是 LevelConfig(继承自scriptable类型) 的实例了
        return AssetModule.LoadAsset<LevelConfig>($"Level/{level}.asset");
    }



    //--------------<泛型版本尝试>----------------

    //public static GameObject LoadPrefab<Tprefab>(string name)
    //{
    //    string root = Tprefab.ToString();
    //    return AssetModule.LoadAsset<GameObject>($"Prefab/{name}.prefab");
    //}

}
