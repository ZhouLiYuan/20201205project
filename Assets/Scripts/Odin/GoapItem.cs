//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Sirenix.OdinInspector;
//using Sirenix.OdinInspector.Editor;
//using Sirenix.Utilities.Editor;
//using Sirenix.Serialization;
//using UnityEditor;

//// 官方教程https://www.youtube.com/watch?v=X0pfuJ6c0T0
////看到5分时没法再看，太多编译错误和脚本缺漏了

////泛型类主要是为了给 泛型成员 传类型参数
////Color Palette Attribute相关教程https://www.jianshu.com/p/9a4a1ec72363
//public class GoapItem<T> where T : ScriptableObject
//{
//    //Inline一列式
//    [InlineEditor(InlineEditorObjectFieldModes.CompletelyHidden)]
//    public T m_selected;


//    //ColorGroupAttribute 好像是要自己创建的特性
//    //详见教程https://blog.csdn.net/qq_26318597/article/details/106209304
//    //正确的创建方法https://kzclip.com/video/31hLz9jK5Rc/create-a-custom-group-with-unity-and-odin-inspector.html
//    [LabelWidth(100)]
//    [PropertyOrder(-1)]
//    [ColorGroupAttribute("CreateNew", 1f, 1f, 1f)]
//    [HorizontalGroup("CreateNew/Horizontal")]
//    public string nameForNew;

//    public string m_path;

//    //对应上面"新规创建"的方法
//    [HorizontalGroup("CreateNew/Horizontal")]
//    [GUIColor(0.7f,0.7f,1f)]
//    [Button]
//    public void CreateNew() 
//    {
//        if (nameForNew == "") return;

//        T newItem = ScriptableObject.CreateInstance<T>();
//        newItem.name = "新的" + typeof(T).ToString();

//        if (m_path == "") { m_path = "资产/"; }
//        //在此路径下创建一个新资源
//        AssetDatabase.CreateAsset(newItem, m_path + "\\" + nameForNew + ".asset");
//        //将所有未保存的资源更改写入磁盘（有点像saveproject？）
//        AssetDatabase.SaveAssets();

//        //重置
//        nameForNew = "";
//    }

//    [HorizontalGroup("CreateNew/Horizontal")]
//    [GUIColor(0.7f, 0.7f, 1f)]
//    [Button]
//    public void DeleteSelected() 
//    {
//        if (m_selected != null)
//        {
//            //返回相对于存储资源的项目文件夹的路径名称
//            string _path = AssetDatabase.GetAssetPath(m_selected);
//            AssetDatabase.DeleteAsset(_path);
//            AssetDatabase.SaveAssets();
//        }
//    }

//    public void SetSelected(object item) 
//    {
//        var attempt = item as T;
//        if (attempt != null)
//            this.m_selected = attempt;
//    }

//    public void SetPath(string path)
//    {
//        this.m_path =  path;
//    }

//}


//[CreateAssetMenu(fileName = "ColorData", menuName = "My Game/Color Data")]
//public class ColorData : ScriptableObject
//{
//    public Color colorValue;
//}

//[CreateAssetMenu(fileName = "ModuleData", menuName = "My Game/ModuleData")]
//public class ModuleData : ScriptableObject
//{
//    public ModuleData m_module;
//}

//public class ColorGroupAttribute : PropertyGroupAttribute
//{
//    public float R, G, B, A;
//    public ColorGroupAttribute(string path) : base(path) 
//    {
//    }
//    //查查float的数值和0~255有什么关系
//    public ColorGroupAttribute(string path,float r, float g, float b, float a = 1f) : base(path)
//    {
//        this.R = r;
//        this.G = g;
//        this.B = b;
//        this.A = a;
//    }
//}

