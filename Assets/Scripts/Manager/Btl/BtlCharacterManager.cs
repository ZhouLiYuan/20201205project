using Role.SelectableRole;
using Role.SpineRole;
using System.Collections.Generic;
using UnityEngine;

//模式区别可以参考 火影忍者究极风暴
//也许角色命名也可以用AC BC加以区分
//多人模式API(封顶四个角色)  可以和电脑对战(最多可以有三个电脑，两个队友或三个敌人)
public class BtlCharacterManager
{
    public static PlayerRole_Spine p1_Role;
    public static PlayerRole_Spine p2_Role;
    public static PlayerRole_Spine p3_Role;
    public static PlayerRole_Spine p4_Role;
    public static Dictionary<string, PlayerRole_Spine> NameDic;


    public static void Init() 
    {
        NameDic = new Dictionary<string, PlayerRole_Spine>();
        p1_Role = new PlayerRole_Spine();
        SpawnCharacter(p1_Role);
        p2_Role = new PlayerRole_Spine();
        SpawnCharacter(p2_Role);
    }


    public static void AddCharacter() 
    {
        int elementCount = 0;
        foreach (KeyValuePair< string, PlayerRole_Spine> KVP in NameDic){elementCount++;}
        if (elementCount >= 4) return;//上限只能四个人玩
        if (elementCount == 3)
        {
            p3_Role = new PlayerRole_Spine();
            SpawnCharacter(p3_Role);
        }
        else if (elementCount == 4)
        {
            p4_Role = new PlayerRole_Spine();
            SpawnCharacter(p4_Role);
        }
        else { return; }
    }

    /// <summary>
    /// 加载并生成 主角Gobj，并创建 主角脚本实例
    /// (根据AssetBundles_sai文件夹下prefab名)
    /// </summary>
    /// <returns></returns>
    public static PlayerRole_Spine SpawnCharacter(PlayerRole_Spine role)
    {
        //加载配置
        //var config = ResourcesLoader.LoadConfigByID<PlayerRoleConfig>(0);
        var prefab = ResourcesLoader.LoadSpineRolePrefab();
        //初始化角色
        var p1_gobj = Object.Instantiate(prefab);
        //p1_gobj.name = config.AssetName;
        role.Init(p1_gobj);
        //role.InitProperties(config);
        //添加到字典
        NameDic[p1_gobj.name] = role;
        return role;
    }
}
#region 添加受击框API(other Version)
//public static Collider2D p1_HitCollider;
//public static Collider2D[] p1_HitColliders;

//p1_HitCollider = p1_gobj.GetComponent<Collider2D>();//只有单个受击刚体的情况
//p1_HitColliders = p1_gobj.GetComponents<Collider2D>();
//p1_HitColliders = p1_gobj.GetComponentsInChildren<Collider2D>();
#endregion