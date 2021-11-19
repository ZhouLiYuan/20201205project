using UnityEngine;
using UnityEngine.AddressableAssets;

public static class AssetModule 
{
    public static T LoadAsset<T>(string path)
    {
        var handle = Addressables.LoadAssetAsync<T>(path);
        //if(handle.Result == null) return default;//找不到对象时怎么办
        //实现同步加载的API
         return handle.WaitForCompletion();
    }
}
