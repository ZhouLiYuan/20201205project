using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//支持多角色
public static class PlayerManager
{
    private static List<PlayerRole> m_Roles = new List<PlayerRole>();

    /// <summary>
    /// 加载并生成 主角Gobj，并创建 主角脚本实例
    /// </summary>
    /// <returns></returns>
    public static PlayerRole SpawnCharacter(string roleName) 
    {
        var prefab = AssetModule.LoadAsset<GameObject>($"Assets/AssetBundles_sai/{roleName}.prefab");
        //创建逻辑层和表现层实例
        PlayerRole m_Role = new PlayerRole(Object.Instantiate(prefab));
        m_Roles.Add(m_Role);
        return m_Role;
    }

}
