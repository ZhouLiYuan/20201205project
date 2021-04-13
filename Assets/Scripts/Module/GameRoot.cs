using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoot : MonoBehaviour
{
    /// <summary>
    /// 管理全局单例
    /// </summary>
    public static GameRoot Instance { get; private set; }

    public MySceneManager MySceneManager { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            //把自己这个组件传进静态变量（算是一种单例实现方式）
            Instance = this;

        else
            //销毁自己
            Destroy(gameObject);

        MySceneManager = new MySceneManager();
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        MySceneManager.SetScene(new StartMenuScene());
    }

    void Update()
    {

    }
}
