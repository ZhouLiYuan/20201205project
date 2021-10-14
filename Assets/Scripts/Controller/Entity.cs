using UnityEngine;


//适用于 场景中的单体原件

/// <summary>
/// 建立每个单体Gobj（表现层）和脚本（逻辑层）之间的联系
/// </summary>
public class Entity
{

    public GameObject GameObject { get; private set;}
    public Transform Transform { get; private set;}

    //配置表基本属性
    public int assetID;
    public string UniqueName =>GameObject.name; //注意脚本的name和实际GameObject的name并不是一回事
    public string assetName;

    //---------------------<初始化方式二选一>-----------------------
    internal Entity(GameObject obj) 
    {
        GameObject = obj;
        Transform = obj.transform;
    }

    //Init()初始化 (供需要无参构造函数的泛型方法使用)
    internal Entity() { }
    public virtual void Init(GameObject obj)
    {
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
        assetID = config.AssetID;
        assetName = config.AssetName;

        //assetID = 0;
        //为了动画层级（确保生成的时候名字不带[clone]）
        //assetName = "NPC";
    }




    /// <summary>
    /// 只能查找子物体，以及子物体的component
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <returns></returns>
    public T Find<T>(string path) where T : Object
    {
        var t = Transform.Find(path);
        if (typeof(T) == typeof(Transform)) return t as T;
        if (typeof(T) == typeof(GameObject)) return t.gameObject as T;
        return t.GetComponent<T>();
    }



}
