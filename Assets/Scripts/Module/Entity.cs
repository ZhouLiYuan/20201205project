using UnityEngine;


//适用于 场景中的单体原件

/// <summary>
/// 建立每个单体Gobj（表现层）和脚本（逻辑层）之间的联系
/// </summary>
public class Entity
{
    /// <summary>
    /// topNodeTransform
    /// </summary>
    public Transform Transform { get; private set; }
    public GameObject GameObject { get; private set;}
  
    //配置表基本属性
    public int AssetID{ get; private set; }
    //实例名(用于做字典key)
    public string UniqueName =>GameObject.name; //注意脚本的name和实际GameObject的name并不是一回事
    public string AssetName { get; private set; }

    //---------------------<初始化方式二选一>-----------------------
    internal Entity(GameObject obj) 
    {
        MakeZAxisZero(obj);
        GameObject = obj;
        Transform = obj.transform;
    }

    //Init()初始化 (供需要无参构造函数的泛型方法使用)
    internal Entity() { }
    public virtual void Init(GameObject obj)
    {
        MakeZAxisZero(obj);
        GameObject = obj;
        Transform = obj.transform;
    }

    //---------------------<配置表初始化>-----------------------
    /// <summary>
    /// 适用于Json反序列化
    /// </summary>
    /// <param name="config"></param>
    public virtual void InitProperties(Config config) 
    {
        AssetID = config.AssetID;
        AssetName = config.AssetName;

        //assetID = 0;
        //为了动画层级（确保生成的时候名字不带[clone]）
        //assetName = "NPC";
    }

    /// <summary>
    /// 只能查找子物体，以及子物体的component
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="childPath"></param>
    /// <returns></returns>
    public T Find<T>(string childPath) where T : Object
    {
        var t = Transform.Find(childPath);
        if (!t) { Debug.LogError($"can not find child in {childPath}"); return null; }
        if (typeof(T) == typeof(Transform)) return t as T;
        if (typeof(T) == typeof(GameObject)) return t.gameObject as T;
        return t.GetComponent<T>();
    }

    private static void MakeZAxisZero(GameObject obj)
    {
        Vector3 posWithoutZ = obj.transform.position;
        posWithoutZ.z *= 0f;
        obj.transform.position = posWithoutZ;
    }
}
