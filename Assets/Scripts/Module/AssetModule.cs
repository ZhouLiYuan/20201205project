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

    #region 更详细可见项目(待完善的万能加载)
    private static int loadCount = 0;
    public static int LoadCount//暂时不知道有什么用的加载数量
    {
        get { return loadCount; }
        private set { loadCount = value; }
    }

    // assetName尽量打后缀名(除非确保该资源类型在该路径文件夹中唯一)
    //cb用于取出返回值
    public static void LoadAsset(string path, string assetName, System.Type type,System.Action<object>cb, bool isCache)
    {
        Debug.Log($"加载资源{assetName}");


        #region 
        string categoryName = "";//""就是String的Empty
        //如果没有找到对应字符会返回-1
        int index = path.LastIndexOf('/');
        if (index != -1)
        {
            categoryName = path.Substring(index + 1);//获得相对路径名
        }
        #endregion


        //index重复利用
        CheckOrFixAssetName(assetName, type);


        LoadCount++;
        //本地委托
        System.Action<object> _cb = (_obj) =>
         {
             cb(_obj);
             LoadCount--;
         };

        if (string.IsNullOrEmpty(assetName))
        {
        }
    }
    #endregion

    /// <summary>
    /// 查看是否包含后缀名
    /// </summary>
    /// <param name="assetName"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static string CheckOrFixAssetName(string assetName, System.Type type)
    {
        #region 确保后缀名
         int  index = assetName.LastIndexOf('.');
        if (index == -1) //不包含时就补上
        {
            if (type == typeof(GameObject)) { assetName += ".prefab"; }
            else if (type == typeof(AnimationClip)) { assetName += ".anim"; }
            else if (type == typeof(Sprite)) { assetName += ".png"; }//需要确保项目中只有png没有jpg之类其他的
            else if (type == typeof(Material)) { assetName += ".mat"; }
            else if (type == typeof(AudioClip)) { assetName += ".wav"; }
            else { Debug.LogError($"项目中不存在{type}资源类型"); }
            //缺点:项目中 每种type的Asset只能对应单一 数据类型
        }
        #endregion
        return assetName;
    }
}
