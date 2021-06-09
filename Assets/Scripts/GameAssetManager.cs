using UnityEngine.InputSystem;
using UnityEngine;


/// <summary>
/// 再抽象多一层 游戏资源加载管理
/// </summary>
public class GameAssetManager
{
    public static InputActionAsset LoadInputActionAsset(string path)
    {
        return AssetModule.LoadAsset<InputActionAsset>(path);
    }
}
