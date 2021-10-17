using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//支持多角色
//可以通过odin序列化静态变量
//让PlayerRole实例成为全局唯一
public static class PlayerManager
{
    public static Dictionary<GameObject, PlayerRole> roles = new Dictionary<GameObject, PlayerRole>();
    private static List<PlayerRole> m_Roles = new List<PlayerRole>();//多人模式才会用得上，也有很多配套的API需要改
    public static PlayerRole m_Role;
    public static string m_RoleName = "character";
    public static GameObject m_gobj;
    public static Collider2D pl_HitCollider;


    /// <summary>
    /// 加载并生成 主角Gobj，并创建 主角脚本实例
    /// (根据AssetBundles_sai文件夹下prefab名)
    /// </summary>
    /// <returns></returns>
    public static PlayerRole SpawnCharacter()
    {
        var prefab = ResourcesLoader.LoadPlayerPrefab(m_RoleName);
        //创建逻辑层和表现层实例
        m_gobj = Object.Instantiate(prefab);
        m_Role = new PlayerRole(m_gobj);
        m_Roles.Add(m_Role);
        roles[m_gobj] = m_Role;

        // 设置受击框collider
        pl_HitCollider = m_gobj.GetComponent<BoxCollider2D>();
        return m_Role;
    }

}
