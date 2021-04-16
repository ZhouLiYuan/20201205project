using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class WaitLoadPanel<T> : CustomYieldInstruction where T : BasePanel 
{
    /// <summary>
    /// 等待加载的资源
    /// </summary>
    public T m_asset;

    public bool IsCompleted { get; private set; } = false;
    public override bool keepWaiting => !IsCompleted;

    public void Complete(T asset)
    {
        IsCompleted = true;
        m_asset = asset;
    }

    //暂时还没用上的字段

    /// <summary>
    /// 用于释放的句柄 
    /// </summary>
    public AsyncOperationHandle<T> Handle;

    /// <summary>
    ///  协程结束时的回调
    /// </summary>
    public event Action OnFinish;

    /// <summary>
    /// 加载资源成功时的回调
    /// </summary>
    public event Action<T> OnLoadSucceed;

}
