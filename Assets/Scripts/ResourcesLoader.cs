using System;
using System.Collections.Generic;
using UnityEngine;

//封装路径
public static class ResourcesLoader
{
    private static EnemyConfig[] enemyConfigs;
    private static WeaponConfig[] weaponConfigs;

    private static NPCConfig[] NPCConfigs;

    public static GameObject LoadPlayerPrefab(string name)
    {
        return AssetModule.LoadAsset<GameObject>($"Player/{name}.prefab");
    }

    public static DialogueConfig[] LoadDialogue(int episodeID)
    {
        var json = AssetModule.LoadAsset<TextAsset>($"Dialogue/{episodeID}.json").text;
        var dialogueConfigs = Newtonsoft.Json.JsonConvert.DeserializeObject<DialogueConfig[]>(json);
        List<DialogueConfig> results = new List<DialogueConfig>();
        for (int i = 0; i < dialogueConfigs.Length; i++)
        {
            if (dialogueConfigs[i].EpisodeID != episodeID) continue;
            //强行用上顺序（暂时可有可无）
            results.Insert(dialogueConfigs[i].Sequence, dialogueConfigs[i]);
            //results[dialogueConfigs[i].Sequence] = dialogueConfigs[i];
            //results.Add(dialogueConfigs[i]);
        }
        return results.ToArray();
    }

    public static GameObject LoadPrefab(string name)
    {
        return AssetModule.LoadAsset<GameObject>($"Prefab/{name}.prefab");
    }

    //Panel
    public static GameObject LoadPanelPrefab(string name)
    {
        return AssetModule.LoadAsset<GameObject>($"Panel/{name}.prefab");
    }

    //Effect
    public static GameObject LoadEffectPrefab(string name)
    {
        return AssetModule.LoadAsset<GameObject>($"Effect/{name}.prefab");
    }

    //Enemy json配置
    public static EnemyConfig LoadEnemyConfigByID(int id)
    {
        if (enemyConfigs == null)
        {
            var json = AssetModule.LoadAsset<TextAsset>($"Config/EnemyConfig.json").text;
            enemyConfigs = Newtonsoft.Json.JsonConvert.DeserializeObject<EnemyConfig[]>(json);
        }
        for (int i = 0; i < enemyConfigs.Length; i++)
        {
            if (enemyConfigs[i].AssetID == id) return enemyConfigs[i];
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
            if (enemyConfigs[i].AssetName == name) return enemyConfigs[i];
        }
        return null;
    }
    public static GameObject LoadEnemyPrefab(string name)
    {
        return AssetModule.LoadAsset<GameObject>($"Enemy/{name}.prefab");
    }

    //Weapon json配置
    public static WeaponConfig LoadWeaponConfigByID(int id)
    {
        if (weaponConfigs == null)
        {
            var json = AssetModule.LoadAsset<TextAsset>($"Config/WeaponConfig.json").text;
            weaponConfigs = Newtonsoft.Json.JsonConvert.DeserializeObject<WeaponConfig[]>(json);
        }
        for (int i = 0; i < weaponConfigs.Length; i++)
        {
            if (weaponConfigs[i].AssetID == id) return weaponConfigs[i];
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
            if (weaponConfigs[i].AssetName== name) return weaponConfigs[i];
        }
        return null;
    }
    public static GameObject LoadWeaponPrefab(string name)
    {
        return AssetModule.LoadAsset<GameObject>($"Weapon/{name}.prefab");
    }

    //Projectile
    public static GameObject LoadProjectilePrefab(string name)
    {
        return AssetModule.LoadAsset<GameObject>($"Weapon/Projectile/{name}.prefab");
    }

    //NPC json配置
    public static NPCConfig LoadNPCConfigByName(string name)
    {
        if (NPCConfigs == null)
        {
            var json = AssetModule.LoadAsset<TextAsset>($"Config/NPCConfig.json").text;
            NPCConfigs = Newtonsoft.Json.JsonConvert.DeserializeObject<NPCConfig[]>(json);
        }
        for (int i = 0; i < NPCConfigs.Length; i++)
        {
            if (NPCConfigs[i].AssetName == name) return NPCConfigs[i];
        }
        return null;
    }
    public static GameObject LoadNPCPrefab(string name)
    {
        return AssetModule.LoadAsset<GameObject>($"NPC/{name}.prefab");
    }


    //用scriptable asset配置
    public static LevelConfig LoadLevelConfig(int level)
    {
        //project中的scriptable asset其实就是 LevelConfig(继承自scriptable类型) 的实例了
        return AssetModule.LoadAsset<LevelConfig>($"Level/{level}.asset");
    }


    //Guider
    public static GameObject LoadGuiderPrefab(string name)
    {
        return AssetModule.LoadAsset<GameObject>($"Guider/{name}.prefab");
    }

    public static GameObject LoadHealthBarPrefab()
    {
        return AssetModule.LoadAsset<GameObject>("HealthBar");
    }

    public static Sprite LoadSprite(string name)
    {
        return AssetModule.LoadAsset<Sprite>($"Sprite/{name}.png");
    }

    public static Sprite LoadHeadIcon(string name, string emotion)
    {
        return AssetModule.LoadAsset<Sprite>($"Sprite/HeadIcon/{name}/{emotion}.png");
    }

    public static Material LoadMaterial(string name)
    {
        return AssetModule.LoadAsset<Material>($"Material/{name}.mat");
    }

    //anim
    public static AnimationClip LoadAnimClip(string animatorName ,string clipName)
    {
        var _index = animatorName.IndexOf("_");
        var type = animatorName.Substring(0, _index - 1);
        var category = animatorName.Substring(_index + 1);
        return AssetModule.LoadAsset<AnimationClip>($"Assets/AssetBundles_sai/Animation/{type}/{category}/{clipName}.anim");
    }

    //Sound
    public static GameObject LoadSEPrefab(string name = "SE_Prefab") 
    {
        return AssetModule.LoadAsset<GameObject>($"Sound/{name}.prefab");
    }


    //--------------<泛型版本尝试>----------------

    //需要文件夹名和类型名统一
    public static GameObject LoadTPrefab<Tprefab>(string name)
    {
        string TypeName = typeof(Tprefab).Name;
        return AssetModule.LoadAsset<GameObject>($"{TypeName}/{name}.prefab");
    }

    //缺点 无法set外部configs字段
    public static TConfig LoadConfigByID<TConfig>(int id) where TConfig : Config, new()
    {
        List<TConfig> configs = new List<TConfig>();
        var configName = typeof(TConfig).Name;
        configs.Clear();
        if (configs.Count == 0)
        {
            var json = AssetModule.LoadAsset<TextAsset>($"Config/{configName}.json").text;
            configs = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TConfig>>(json);
        }
        for (int i = 0; i < configs.Count; i++)
        {
            if (configs[i].AssetID == id) return configs[i];
        }
        return null;
    }

}
