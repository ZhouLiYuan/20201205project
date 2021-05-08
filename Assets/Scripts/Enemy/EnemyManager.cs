using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyManager
{
    public static IEnumerator InstantiateEnemyByCoroutine<TEnemy>(System.Action<TEnemy> onInstantiated = null) where TEnemy : BaseEnemy, new() 
    {
        //panel并不作为component，只是需要一个地方存放 脚本类实例，用new不用AddComponent
        var enemy = new TEnemy();

        ////用协程方式加载
        var loadPrefabHandle = new WaitLoadAsset<GameObject>(enemy.Path);


        System.Action<GameObject> instantiateEnemylAction = prefab =>
        {
            GameObject enemyGobj = Object.Instantiate(prefab);
            //加载完面板后初始化面板脚本中的字段
            //因为 类型名就是面板名，所以可以typeof(TPanel).Name
            enemyGobj.name = typeof(TEnemy).Name;
            enemy.Init(enemyGobj.name, enemyGobj);
            
        };

        yield return loadPrefabHandle;
        //return new WaitInstantiateEnemy<TEnemy>(loadPrefabHandle, instantiateEnemylAction, onInstantiated);
    }
}
