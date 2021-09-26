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

    /// <summary>
    /// string是EnemyObj名（表现层），GameObject是实例化敌人Gobj
    /// </summary>
    public static Dictionary<string, GameObject> enemyDic = new Dictionary<string, GameObject>();
    public static Dictionary<Collider2D,GameObject> en_colliderDic = new Dictionary<Collider2D, GameObject>();

    static EnemyManager() 
    {
        enemiesList = new GameObject("EnemiesList");
        enemiesListTransform = enemiesList.transform;
    }


    /// <summary>
    /// TEnemy是脚本类型(逻辑层)，string是prefab名（表现层预制件）
    /// 调用一次生成一个敌人并添加到字典
    /// </summary>
    /// <see cref ="m_name"/>
    /// <typeparam name="TEnemy"></typeparam>
    /// <param name="prefabName"></param>
    /// <returns></returns>
    public static TEnemy SpawnEnemy<TEnemy>(string prefabName) where TEnemy : BaseEnemy,new()
    {
        var prefab = ResourcesLoader.LoadEnemyPrefab(prefabName);
        var obj = Object.Instantiate(prefab,enemiesListTransform);
        TEnemy enemy = new TEnemy();

        //初始化脚本component变量
        //传入的参数name只是prefab名（一个兵种的名称），但会有多个同种兵（所以就用场景中生成的Gobj命名）
        enemy.Init(obj.name,obj);
       
     
        return enemy;
    }
}
