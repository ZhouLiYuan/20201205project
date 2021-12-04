using App.Sound;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;




// BuildMenuTree用到的示例类
public class TextureUtilityEditor
{
    [BoxGroup("Tool"), HideLabel, EnumToggleButtons]
    public Tool Tool;

    public List<Texture> Textures;

    [Button(ButtonSizes.Large), HideIf("Tool", Tool.Rotate)]
    public void SomeAction() { }

    [Button(ButtonSizes.Large), ShowIf("Tool", Tool.Rotate)]
    public void SomeOtherAction() { }
}


//在编辑器窗口中渲染实例对象(在project中选中的)目前只能渲染 脚本和 ScriptableObject
public class MyTargetEditorWindow : OdinEditorWindow
{
    [MenuItem("Tools/My Target Editor")]
    private static void OpenWindow()
    {
        GetWindow<MyTargetEditorWindow>().Show();
    }

    protected override void Initialize()
    {
        this.WindowPadding = Vector4.zero;
    }

    protected override object GetTarget()
    {
        return Selection.activeObject;
    }
}


//Odin窗口的菜单栏选项
public class OdinWindowItem<T> where T : ScriptableObject
{
    //Inline一列式
    [InlineEditor(InlineEditorObjectFieldModes.CompletelyHidden)]
    public T selectedItem;


    [LabelWidth(100)]
    [PropertyOrder(-1)]
    //[HorizontalGroup("CreateNew/Horizontal")]
    [InlineButton("CreateNew", "新建Asset")]
    [InlineButton("DeleteSelected", "删除")]
    [LabelText("新建资产命名")]
    public string newItemName;

    [LabelWidth(100)]
    [PropertyOrder(0)]
    [LabelText("资产保存路径")]
    public string m_path;

    //对应上面"新规创建"的方法
    //[HorizontalGroup("CreateNew/Horizontal")]
    [LabelText("新建资产")]
    [GUIColor(0.7f, 0.7f, 1f)]
    [Button]
    public void CreateNew()
    {
        if (newItemName == "") return;

        T newItem = ScriptableObject.CreateInstance<T>();
        newItem.name = "新的" + typeof(T).ToString();

        if (m_path == "") { m_path = "资产/"; }

        AssetDatabase.CreateAsset(newItem, m_path + "\\" + newItemName + ".asset"); //在指定路径下创建一个新资源
        AssetDatabase.SaveAssets();//将所有未保存的资源更改写入磁盘（save project）

        //重置
        newItemName = "";
    }



     //-------------------------------------------下面的三个方法暂时不知道怎么用
    //[LabelText("设置路径")]
    //[GUIColor(0.7f, 0.7f, 1f)]
    //[Button]
    public void SetPath(string path)
    {
        this.m_path = path;
    }

    //[LabelText("设置为选中资产")]
    //[GUIColor(0.7f, 0.7f, 1f)]
    //[Button]
    public void SetSelected(object item)
    {
        Debug.Log("SetSelected被调用");
        var attempt = item as T;
        if (attempt != null)
            this.selectedItem = attempt;
    }

    //[HorizontalGroup("CreateNew/Horizontal")]
    [LabelText("删除选中资产")]
    [GUIColor(0.7f, 0.7f, 1f)]
    [Button]
    public void DeleteSelected()
    {
        if (selectedItem != null)
        {
            //返回相对于存储资源的项目文件夹的路径名称
            string _path = AssetDatabase.GetAssetPath(selectedItem);
            AssetDatabase.DeleteAsset(_path);
            AssetDatabase.SaveAssets();
        }
    }

}

//自定义特性
public class ColorGroupAttribute : PropertyGroupAttribute
{
    public float R, G, B, A;
    public ColorGroupAttribute(string path) : base(path)
    {
    }
    //查查float的数值和0~255有什么关系
    public ColorGroupAttribute(string path, float r, float g, float b, float a = 1f) : base(path)
    {
        this.R = r;
        this.G = g;
        this.B = b;
        this.A = a;
    }
}