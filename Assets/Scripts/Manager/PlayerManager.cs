using Role.SelectableRole;
using System.Collections.Generic;
using UnityEngine;


//支持多角色
//可以通过odin序列化静态变量
//让PlayerRole实例成为全局唯一
public static class PlayerManager
{
    public static string  p1_RoleName;
    public static PlayerRole p1_Role;
    public static GameObject p1_gobj;
    public static Collider2D p1_HitCollider;

    //多人模式API
    public static Dictionary<GameObject, PlayerRole> roles = new Dictionary<GameObject, PlayerRole>();
    private static List<PlayerRole> m_Roles = new List<PlayerRole>();


    /// <summary>
    /// 加载并生成 主角Gobj，并创建 主角脚本实例
    /// (根据AssetBundles_sai文件夹下prefab名)
    /// </summary>
    /// <returns></returns>
    public static PlayerRole SpawnPlayer1()
    {
        var config = ResourcesLoader.LoadConfigByID<PlayerRoleConfig>(0);
        var prefab = ResourcesLoader.LoadPlayerPrefab(config.AssetName);
        p1_RoleName = config.AssetName;
       //创建逻辑层和表现层实例
       p1_gobj = Object.Instantiate(prefab);
        p1_Role = new PlayerRole();
        p1_gobj.name = p1_RoleName;
        p1_Role.Init(p1_gobj);
        p1_Role.InitProperties(config);

        m_Roles.Add(p1_Role);
        roles[p1_gobj] = p1_Role;

        // 设置受击框collider
        p1_HitCollider = p1_gobj.GetComponent<BoxCollider2D>();

        var firstWeaponConfig = ResourcesLoader.LoadWeaponConfigByID(1);//临时用
        var secondWeaponConfig = ResourcesLoader.LoadWeaponConfigByID(2);
        var thridWeaponConfig = ResourcesLoader.LoadWeaponConfigByID(3);
        p1_Role.EquipWeapon(firstWeaponConfig);
        p1_Role.EquipWeapon(secondWeaponConfig);
        p1_Role.EquipWeapon(thridWeaponConfig);

        return p1_Role;
    }

    //public static PlayerRole SpawnCharacter(int characterID = 0)
    //{
    //    var config = ResourcesLoader.LoadConfigByID<PlayerRoleConfig>(characterID);
    //    var prefab = ResourcesLoader.LoadPlayerPrefab(config.AssetName);
    //    m_RoleName = config.AssetName;
    //    //创建逻辑层和表现层实例
    //    m_gobj = Object.Instantiate(prefab);
    //    m_Role = new PlayerRole();
    //    m_gobj.name = m_RoleName;
    //    m_Role.Init(m_gobj);
    //    m_Role.InitProperties(config);

    //    m_Roles.Add(m_Role);
    //    roles[m_gobj] = m_Role;

    //    // 设置受击框collider
    //    pl_HitCollider = m_gobj.GetComponent<BoxCollider2D>();

    //    var firstWeaponConfig = ResourcesLoader.LoadWeaponConfigByID(1);//临时用
    //    var secondWeaponConfig = ResourcesLoader.LoadWeaponConfigByID(2);
    //    var thridWeaponConfig = ResourcesLoader.LoadWeaponConfigByID(3);
    //    m_Role.EquipWeapon(firstWeaponConfig);
    //    m_Role.EquipWeapon(secondWeaponConfig);
    //    m_Role.EquipWeapon(thridWeaponConfig);

    //    return m_Role;
    //}

}
