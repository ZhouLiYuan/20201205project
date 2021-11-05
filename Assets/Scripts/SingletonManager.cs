using Role;
using Role.NPCs;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SingletonManager<TEntity> where TEntity : Entity, new()
{
    public static GameObject s_list;
    public static Transform s_listTransform;

    //提供不同的查询获得T的方式
    public static Dictionary<string, TEntity> nameDic = new Dictionary<string, TEntity>();
    public static Dictionary<Collider2D, TEntity> hitColliderDic = new Dictionary<Collider2D, TEntity>();

    static SingletonManager()
    {
        string typeName = typeof(TEntity).Name;
        s_list = new GameObject($"{typeName}List");
        s_listTransform = s_list.transform;
        s_listTransform.position = Vector3.zero;
        s_listTransform.rotation = new Quaternion(0, 0, 0, 0);
    }

    public SingletonManager() { }

    public static TEntity GetInstanceByName(string name)
    {
        if (nameDic.TryGetValue(name, out var T))
        {
            return T;
        }
        return default(TEntity);
    }

    public static TEntity GetInstanceByCollider(Collider2D instanceCollider)
    {
        if (hitColliderDic.TryGetValue(instanceCollider, out var Instance))
        {
            return Instance;
        }
        return default(TEntity);
    }

    /// <summary>
    /// 不涉及动画层级的Entity才能用（不需要通过固定名字识别）
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="prefabName"></param>
    /// <param name="bornPoint"></param>
    /// <returns></returns>
    public static TEntity SpawnInstance(string prefabName, Vector3 bornPoint)
    {
        Quaternion rotation = new Quaternion(0, 0, 0, 0);
        var prefab = ResourcesLoader.LoadTPrefab<TEntity>(prefabName);
        var obj = Object.Instantiate(prefab, bornPoint, rotation, s_listTransform);
        var instance = RecognizeType(prefabName);
        obj.name = CreateUniqueName(instance.GetType().Name);
        //建立表现层和逻辑层联系
        instance.Init(obj);
        nameDic[obj.name] = instance;
        return instance;
    }

    public static string CreateUniqueName(string typeName)
    {
        var name = typeName;
        var keys = new string[nameDic.Keys.Count];
        nameDic.Keys.CopyTo(keys, 0);
        var uniqueName = ObjectNames.GetUniqueName(keys, name);
        return uniqueName;
    }

    private static TEntity RecognizeType(string prefabName)
    {
        var typeName = typeof(TEntity).ToString();

        //注意命名空间也要附上Role.Enemy
        //或者可以用保险方法typeof(Enemy).ToString()
        if (typeName == typeof(Enemy).ToString())
            switch (prefabName)
            {
                case "enemy_sword":
                    return new SwordEnemy() as TEntity;//不知道会不会发生数据丢失
                case "enemy_gun":
                    return new GunEnemy() as TEntity;
                default:
                    Debug.LogError($"不存在对应名称{typeName}");
                    return new Enemy() as TEntity;
            }

        else if (typeName == typeof(NPC).ToString())
            switch (prefabName)
            {
                default:
                    //Debug.LogError($"不存在对应名称{typeName}");
                    return new NPC() as TEntity;
            }

        else return new Entity() as TEntity;
    }

}

/// <summary>
/// 敌人规则命名 Enemy_ attacker名 
/// 生成不同类型的敌人，并把敌人存入现有字典中
/// </summary>
public class EnemyManager : SingletonManager<Enemy>
{
    //public static GameObject enemiesList;
    //public static Transform enemiesListTransform;

    //提供不同的查询获得敌人的方式
    //public static Dictionary<string, BaseEnemy> en_nameDic = new Dictionary<string, BaseEnemy>();
    //public static Dictionary<Collider2D, BaseEnemy> en_hitColliderDic = new Dictionary<Collider2D, BaseEnemy>();

    //static EnemyManager() 
    //{
    //    enemiesList = new GameObject("EnemiesList");
    //    enemiesListTransform = enemiesList.transform;
    //    enemiesListTransform.position = Vector3.zero; 
    //    enemiesListTransform.rotation = new Quaternion(0, 0, 0, 0);
    //}


    ///// <summary>
    ///// TEnemy是脚本类型(逻辑层)，string是prefab名（表现层预制件）
    ///// 调用一次生成一个敌人并添加到字典
    ///// </summary>
    ///// <see cref ="m_name"/>
    ///// <typeparam name="TEnemy"></typeparam>
    ///// <param name="prefabName"></param>
    ///// <returns></returns>
    //public static TEnemy SpawnEnemy<TEnemy>(string prefabName,Vector3 bornPoint) where TEnemy : BaseEnemy,new()
    //{
    //    Quaternion rotation = new Quaternion(0, 0, 0, 0);
    //    var prefab = ResourcesLoader.LoadEnemyPrefab(prefabName);
    //    var obj = Object.Instantiate(prefab,bornPoint,rotation,enemiesListTransform);
    //    TEnemy enemy = new TEnemy();
    //    //初始化脚本component变量
    //    enemy.Init(obj);
    //    return enemy;
    //}

    //public static BaseEnemy GetEnemyByCollider(Collider2D instanceCollider)
    //{
    //    if (en_hitColliderDic.TryGetValue(instanceCollider, out var enemyInstance))
    //    {
    //        return enemyInstance;
    //    }
    //    return null;
    //}


    //public static BaseEnemy GetEnemyByName(string instanceName)
    //{
    //    if (en_nameDic.TryGetValue(instanceName, out var enemyInstance))
    //    {
    //        return enemyInstance;
    //    }
    //    return null;
    //}
}

public class NPCManager : SingletonManager<NPC>
{
}