using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyManager
{
    /// <summary>
    /// string是EnemyObj名（表现层），GameObject是实例化敌人Gobj
    /// </summary>
    public static Dictionary<string, GameObject> enemyDic = new Dictionary<string, GameObject>();

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
        var prefab = AssetModule.LoadAsset<GameObject>($"Enemy/enemy_sword{prefabName}.prefab");
        var obj = Object.Instantiate(prefab);
        TEnemy enemy = new TEnemy();

        //初始化脚本component变量
        //传入的参数name只是prefab名（一个兵种的名称），但会有多个同种兵（所以就用场景中生成的Gobj命名）
        enemy.Init(obj.name,obj);
        enemyDic.Add(obj.name, obj);
        return enemy;
    }
    //public static IEnumerator InstantiateEnemyByCoroutine<TEnemy>(System.Action<TEnemy> onInstantiated = null) where TEnemy : BaseEnemy, new() 
    //{
    //    //panel并不作为component，只是需要一个地方存放 脚本类实例，用new不用AddComponent
    //    var enemy = new TEnemy();

    //    ////用协程方式加载
    //    var loadPrefabHandle = new WaitLoadAsset<GameObject>(enemy.Path);

    //    System.Action<GameObject> instantiateEnemylAction = prefab =>
    //    {
    //        GameObject enemyGobj = Object.Instantiate(prefab);
    //        //加载完面板后初始化面板脚本中的字段
    //        //因为 类型名就是面板名，所以可以typeof(TPanel).Name
    //        enemyGobj.name = typeof(TEnemy).Name;
    //        enemy.Init(enemyGobj.name, enemyGobj);

    //    };

    //    yield return loadPrefabHandle;
    //    //return new WaitInstantiateEnemy<TEnemy>(loadPrefabHandle, instantiateEnemylAction, onInstantiated);
    //}
}
