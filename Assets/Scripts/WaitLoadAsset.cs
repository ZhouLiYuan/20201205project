using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

/// <summary>
/// 等待资源加载完成
/// </summary>
/// <typeparam name="T"></typeparam>
public class WaitLoadAsset<T> : CustomYieldInstruction where T : Object
{

    //---------------------需要外传的字段----------------------------
    public T m_asset;
    /// <summary>
    /// 用于释放资源的句柄 
    /// </summary>
    public AsyncOperationHandle<T> m_handle;


    //---------------------协程相关字段----------------------------
    public bool IsCompleted { get; private set; } = false;
    public override bool keepWaiting => !IsCompleted;
    private Coroutine m_coroutine;


    //---------------------异步中的事件----------------------------
    /// <summary>
    ///  协程结束时的回调
    ///  (因为没有用System命名空间需要加上  System.)
    /// </summary>
    public event System.Action<T> OnFinish;
    /// <summary>
    /// 加载资源成功时的回调
    /// </summary>
    public event System.Action<T> OnLoadSucceed;



    public WaitLoadAsset(string location, System.Action<T> onLoadSucceed = null)
    {
        m_coroutine = CoroutineManager.StartCoroutine(AsyncLoad(location, onLoadSucceed));
    }

    /// <summary>
    ///  Addressables协程加载方法
    /// </summary>
    /// <param name="location"></param>
    /// <param name="onLoadSucceed"></param>
    /// <returns></returns>
    private IEnumerator AsyncLoad(string location, System.Action<T> onLoadSucceed = null)
    {
        OnLoadSucceed += onLoadSucceed;
        var handle = Addressables.LoadAssetAsync<T>(location);
        yield return handle;

        IsCompleted = true;
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            m_asset = handle.Result;
            m_handle = handle;
            OnLoadSucceed?.Invoke(m_asset);
        }
        OnFinish?.Invoke(m_asset);
    }


    /// <summary>
    /// 协程枚举器movenext完成时，把asset传出去（该方法未被调用）
    /// </summary>
    /// <param name="asset"></param>
    public void Complete(T asset)
    {
        IsCompleted = true;
        m_asset = asset;
    }

    /// <summary>
    /// 终止协程的方法（该方法未被调用）
    /// </summary>
    public void Stop() 
    {
        CoroutineManager.StopCoroutine(m_coroutine);
    }

}
