using UnityEngine;
using UnityEngine.AddressableAssets;

public static class AssetModule 
{
    public static T LoadAsset<T>(string path)
    {
        var handle = Addressables.LoadAssetAsync<T>(path);
        //实现同步加载的API
        return handle.WaitForCompletion();
    }
}
