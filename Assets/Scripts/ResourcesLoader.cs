using System;
using System.Collections.Generic;
using UnityEngine;

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

    public static void LoadPrefab(string name)
    {
        AssetModule.LoadAsset<GameObject>($"Prefab/{name}");
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

    public static void LoadEnemyPrefab(string name)
    {
        AssetModule.LoadAsset<GameObject>($"Enemy/{name}.prefab");
    }

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

    public static GameObject LoadWeaponPrefab(string name)
    {
        return AssetModule.LoadAsset<GameObject>($"Weapon/{name}.prefab");
    }

    public static LevelData LoadLevelConfig(int level)
    {
        return AssetModule.LoadAsset<LevelData>($"Level/{level}.asset");
    }
}
