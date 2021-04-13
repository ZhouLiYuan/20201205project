using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 场景模板，包含基本的场景状态信息
/// </summary>
public abstract class BaseScene
{
    /// <summary>
    /// 进入场景时执行
    /// </summary>
    public abstract void OnEnter();

    /// <summary>
    /// 退出场景时执行
    /// </summary>
    public abstract void OnExit();
}
