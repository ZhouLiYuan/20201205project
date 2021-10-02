using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 敌人规则命名 Enemy_ attacker名 
/// 生成不同类型的敌人，并把敌人存入现有字典中
/// </summary>
public static class EnemyManager
{

    public static GameObject enemiesList;
    public static Transform enemiesListTransform;

    //提供不同的查询获得敌人的方式
    //public static Dictionary<string, BaseEnemy> en_nameDic = new Dictionary<string, BaseEnemy>();
    public static Dictionary<Collider2D, BaseEnemy> en_hitColliderDic = new Dictionary<Collider2D, BaseEnemy>();

    static EnemyManager() 
    {
        enemiesList = new GameObject("EnemiesList");
        enemiesListTransform = enemiesList.transform;
        enemiesListTransform.position = Vector3.zero; 
        enemiesListTransform.rotation = new Quaternion(0, 0, 0, 0);
    }


    /// <summary>
    /// TEnemy是脚本类型(逻辑层)，string是prefab名（表现层预制件）
    /// 调用一次生成一个敌人并添加到字典
    /// </summary>
    /// <see cref ="m_name"/>
    /// <typeparam name="TEnemy"></typeparam>
    /// <param name="prefabName"></param>
    /// <returns></returns>
    public static TEnemy SpawnEnemy<TEnemy>(string prefabName,Vector3 bornPoint) where TEnemy : BaseEnemy,new()
    {
        Quaternion rotation = new Quaternion(0, 0, 0, 0);
        var prefab = ResourcesLoader.LoadEnemyPrefab(prefabName);
        var obj = Object.Instantiate(prefab,bornPoint,rotation,enemiesListTransform);
        TEnemy enemy = new TEnemy();
        //初始化脚本component变量
        enemy.Init(obj);
        return enemy;
    }

    public static BaseEnemy GetEnemyByCollider(Collider2D instanceCollider)
    {
        if (en_hitColliderDic.TryGetValue(instanceCollider, out var enemyInstance))
        {
            return enemyInstance;
        }
        return null;
    }


    //public static BaseEnemy GetEnemyByName(string instanceName)
    //{
    //    if (en_nameDic.TryGetValue(instanceName, out var enemyInstance))
    //    {
    //        return enemyInstance;
    //    }
    //    return null;
    //}


}
