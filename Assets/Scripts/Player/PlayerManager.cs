using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//目前只有一个角色AkaOni所以，暂时不用泛型
public static class PlayerManager
{
    /// <summary>
    /// 加载并生成 主角Gobj，并创建 主角脚本实例
    /// </summary>
    /// <returns></returns>
    public static PlayerRole SpawnCharacter() 
    {
        //获得表现层实例
        var prefab = AssetModule.LoadAsset<GameObject>("Assets/AssetBundles_sai/character.prefab");
        GameObject characterGobj = Object.Instantiate(prefab);

        //获得脚本实例
        PlayerRole m_Role = new PlayerRole(characterGobj);
        return m_Role;
    }

}
