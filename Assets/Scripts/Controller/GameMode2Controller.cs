using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode2Controller
{
    //沿用现有脚本(Mono)
    //private PlayerRole m_character;

    private GameObject characterGo;
    private GameObject hookGo;
    private GameObject platform01Go;
    private GameObject platform02Go;

    private Vector3 hookStartPos = new Vector3((float)-0.1, (float)-0.2, 0);

    public void StartGame()
    {
        //UIManager.Open<GamePanel>();
        // 初始化
        Init();
    }

    /// <summary>
    /// 初始化场景
    /// </summary>
    private void Init()
    {
        //加载资源 生成实例
        characterGo = Object.Instantiate(AssetModule.LoadAsset<GameObject>("资源路径名"));
        hookGo = Object.Instantiate(AssetModule.LoadAsset<GameObject>("资源路径名"));
        platform01Go = Object.Instantiate(AssetModule.LoadAsset<GameObject>("资源路径名"));
        platform02Go = Object.Instantiate(AssetModule.LoadAsset<GameObject>("资源路径名"));

        //配置钩锁(跟随角色pos) 好像无论写true还是false都没啥影响？
        hookGo.transform.SetParent(characterGo.transform, false);
        hookGo.transform.position = hookStartPos;


    }
}
