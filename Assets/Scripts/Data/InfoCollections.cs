using System;
using UnityEngine;

public class InfoCollections
{

}


[Serializable]
//Transform继承自Component，Component自带gameObject字段
//而GameObject本身拥有transform字段
//component和GameObject继承自Object
public class EntityInfo<TInfoInspector> where TInfoInspector : InfoInspector
{

    
    public Transform transform;
    public Vector3 Position => transform.position;
    public Quaternion Rotation => transform.rotation;
     
    public TInfoInspector infoInspector;
    public int AssetID => infoInspector.AssetID;
    //public Transform transform => infoInspector.transform;
    //public int ID;
    //public EntityInfo() 
    //{
    //     ID = infoInspector.ID;
    //}
}


[Serializable]
public class EnemyInfo : EntityInfo<EnemyInfoInspector>
{
    public int WeaponID =>infoInspector.WeaponID;

    //如果as类型转换失败那么直接方法改成 双类型参数就好
    //（但是infoInspector mono类可以用new来约束吗，虽然并没有用new来创建 infoInspector ）
    
    //为什么用构造函数会报错？
    //public int WeaponID;
    //public EnemyInfo() : base()
    //{
    //    WeaponID = infoInspector.WeaponID;
    //}
}


[Serializable]
public class PlatformInfo 
{
    public string Name;
}

[Serializable]
public class CheckPointInfo
{

}