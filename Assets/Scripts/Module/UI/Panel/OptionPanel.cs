using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionPanel : BasePanel
{
    static readonly string path = "Resources/Prefab/UI/Panel/OptionPanel";

    /// <summary>
    /// 创建UIInfo实例时，会调用其 有参构造函数（需要传入string类型 数据）
    /// </summary>
    public OptionPanel() : base(new UIInfo(path)) { }
}
