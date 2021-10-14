using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SingletonManager<T>
{
    public static GameObject s_list;
    public static Transform s_listTransform;

    public static Dictionary<string, T> nameDic = new Dictionary<string, T>();
    public static Dictionary<Collider2D, T> hitColliderDic = new Dictionary<Collider2D, T>();

    static SingletonManager()
    {
        string typeName = typeof(T).Name;
        s_list = new GameObject($"{typeName}List");
        s_listTransform = s_list.transform;
        s_listTransform.position = Vector3.zero;
        s_listTransform.rotation = new Quaternion(0, 0, 0, 0);
    }

    public SingletonManager() { }

    public static T GetInstanceByName(string name)
    {
        if (nameDic.TryGetValue(name, out var T))
        {
            return T;
        }
        return default(T);
    }

    public static T GetInstanceByCollider(Collider2D instanceCollider)
    {
        if (hitColliderDic.TryGetValue(instanceCollider, out var Instance))
        {
            return Instance;
        }
        return default(T);
    }

    /// <summary>
    /// 不涉及动画层级的Entity才能用（不需要通过固定名字识别）
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="prefabName"></param>
    /// <param name="bornPoint"></param>
    /// <returns></returns>
    public static TEntity SpawnInstance<TEntity>(string prefabName, Vector3 bornPoint) where TEntity : Entity, new()
    {
        Quaternion rotation = new Quaternion(0, 0, 0, 0);
        var prefab = ResourcesLoader.LoadTPrefab<TEntity>(prefabName);
        var obj = Object.Instantiate(prefab, bornPoint, rotation, s_listTransform);
        TEntity instance = new TEntity();
        //建立表现层和逻辑层联系
        instance.Init(obj);


        //nameDic[uniqueName] = this;
        return instance;
    }

    public static string CreateUniqueName()
    {
        //var name = GetType().Name;//静态方法内不能用该API
        var name = typeof(T).ToString();
        var keys = new string[nameDic.Keys.Count];
        nameDic.Keys.CopyTo(keys, 0);
        var uniqueName = ObjectNames.GetUniqueName(keys, name);
        return uniqueName;
    }

}
