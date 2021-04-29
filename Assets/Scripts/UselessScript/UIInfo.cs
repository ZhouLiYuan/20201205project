using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Name属性也不需要了，直接 类名 和 prefab名同名就完事了

///// <summary>
///// 储存单个UI的信息，包括名字与路径（属性 可公开get 只能在类体中set）
///// </summary>
//public class UIInfo
//{
//    /// <summary>
//    /// UI名称
//    /// </summary>
//    public string Name{get;private set ;}

//    public string Path { get; private set; }

//    //字段用于储存 中转信息，参数用于传递信息
    
//    /// <summary>
//    /// 创建实例时需要向构造函数传递Path的 参数
//    /// </summary>
//    public UIInfo(string path) 
//    {
//        Path = path;
//        //+1是为了不要把 / 也算进去，代码计数都是从0开始算，而不是1吧？
//        Name = path.Substring(path.LastIndexOf('/') + 1);
//    }

//}
