using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using Sirenix.OdinInspector.Editor.Modules;
using Role.SelectableRole;
using System;
using Role;
using System.Linq;
using Sirenix.Serialization;
using Sirenix.Utilities;
using App.Sound;

//推荐教程https://www.jianshu.com/p/a37bd00d12f2（官方教程2 3 Part还可以）
//模拟暂停时的Option视窗（暂定运行时和编辑器状态下都可用）

//似乎是每一次保存脚本会 重启新的一轮Editor中的声明周期
public class OdinGameManagerWindow : OdinMenuEditorWindow
{
    #region Unity实现窗口的API
    [MenuItem("Tools/OdinGameManagerWindow")]
    public static void ShowWindow()
    {
        var window = GetWindow<OdinGameManagerWindow>();
        window.Show();
    }

    //OnGUI 可以每帧调用多次(次数是依據事件，與幀無關),這是因為有多次GUI Event,Time.deltaTime 並不是每次 OnGUI 的間隔時間,小心使用
    //分线程执行，执行先后顺序是：Awake线程->Start线程->FixedUpdate线程->Update线程(每帧一次)->LateUpdate线程->OnGUI线程.
    protected override void OnGUI()
    {
        Debug.Log("OnGUI被调用");
        GUI.Box(new Rect(10, 10, 100, 30), "OnGUI 功能测试");//前两个数值是左上角为基准的offset值，后者是长方形大小

        if (CanTreeRebuild && Event.current.type == EventType.Layout)
        {
            ForceMenuTreeRebuild();
            CanTreeRebuild = false;//确保只有切换managerType的时候调用
        }

        SirenixEditorGUI.Title("Option模拟面板", "使用注释待更新", TextAlignment.Center, true);
        EditorGUILayout.Space();

        switch (managerType)
        {
            case ManagerType.SFX:
            case ManagerType.VFX:
            case ManagerType.Extra:
                DrawEditor(enumIndex);
                break;
            default:
                break;
        }
        EditorGUILayout.Space();
        //DrawEditor(0);//加了这个好像会在第一层渲染多一层managerType
        base.OnGUI();
    }
    #endregion

    #region Odin相关虚方法override

    //tips
    //每帧的调用顺序 1OnGUI 2DrawMenu 3DrawEditors   GetTargets()穿插在123之间反复调用（调用次数一致，四者override后未调用对应的base方法都会导致面板渲染失败）
    //DrawEditors GetTargets() 中 实现 某个枚举的渲染情况 或 没有override这个方法的时候似乎就会 把OnGUI显示的东西重复显示一遍 
    //如果有面板选择的枚举类型 GetTargets OnGUI DrawMenu DrawEditors这几个函数都是需要同步更新的 
    //注意API 的名词是复数还是单数（s），功能会有区别


    //不实现这个抽象成员会报错
    //创建的是最左边的纵向选择面板
    protected override OdinMenuTree BuildMenuTree()
    {
        OdinMenuTree tree = new OdinMenuTree(supportsMultiSelect: true);//构造函数写法见官网
        tree.Selection.SupportsMultiSelect = false;//可否多选
        tree.DefaultMenuStyle = OdinMenuStyle.TreeViewStyle;//像unity ProjectWindow一样显示Asset

        //需要配合上ForceMenuTreeRebuild();
        switch (managerType)
        {
            case ManagerType.SFX:
                tree.AddAllAssetsAtPath("SE Assets Menu", "Assets/AssetBundles_sai/Sound/SE", typeof(AudioClip), true, true).AddThumbnailIcons();//显示部分资源文件的略缩图
                break;
            case ManagerType.VFX:
                tree.AddAllAssetsAtPath("VFX Test Assets Menu", "Assets/AssetBundles_sai/Sound", true, true).AddThumbnailIcons();
                break;
            case ManagerType.Extra:
                //-----------------额外菜单------------------------
                //添加面板选项方法1(选项名+实例)
                //tree.Add("Settings", GeneralDrawerConfig.Instance);
                tree.Add("Utilities", new TextureUtilityEditor());
                //tree.Add("Menu/Items/Are/Created/As/Needed", new GUIContent());
                //tree.Add("Menu/Items/Are/Created", new GUIContent("And can be overridde"));

                //添加面板选项方法2(选项名+路径名+（类型名，可选）+是否包含子路径 + 是否子路径的所有可用类都在一个层级中渲染)
                tree.AddAllAssetsAtPath("Assets Menu", "Assets", true, true);
                //tree.AddAllAssetsAtPath("Odin Settings", "Assets/Plugins/Sirenix", typeof(ScriptableObject), true, true);
                //ShowAllAssets(tree);

                //添加面板选项方法3(创建新的OdinMenuItem)
                var customMenuItem = new OdinMenuItem(tree, "新插入面板", tree.DefaultMenuStyle);
                tree.MenuItems.Insert(2, customMenuItem);//在List<OdinMenuItem>的指定位置中插入新的Item（选项块）

                //tree.EnumerateTree().AddThumbnailIcons();//显示所有资源文件的略缩图
                break;
            default:
                break;
        }



        return tree;
    }

    private void ShowAllAssets(OdinMenuTree tree)
    {
        //获得Assets文件夹下的资源的路径
        var allAssetsPath = AssetDatabase.GetAllAssetPaths()
.Where(x => x.StartsWith("Assets/"))
.OrderBy(x => x);
        //是否显示所有的Assets文件夹下的资源
        foreach (var path in allAssetsPath) { tree.AddAssetAtPath(path.Substring("Assets/".Length), path); }

    }

    protected override void Initialize()//在OnEnable OnGUI后执行 初始化
    {
        //this.WindowPadding = Vector4.zero;

        //ScriptableObject版本
        drawLevelItem.SetPath("Assets/Scripts/Odin");//实验阶段
                                                     //drawSoundItem.SetPath("Assets/Scripts/Odin");

        //场景中mono单例版本
        soundManagerPanel.FindMyObject();


    }


    //protected override void DrawMenu()//似乎每帧都会调用
    //{
    //    base.DrawMenu();
    //    Debug.Log("DrawMenu!!!");
    //    switch (managerType)
    //    {
    //        case ManagerType.SFX:
    //        case ManagerType.VFX:
    //            base.DrawMenu();
    //            break;
    //        default:
    //            break;
    //    }
    //}

    protected override void DrawEditors()//似乎每帧都会调用
    {
        Debug.Log("DrawEditors!!!");
        switch (managerType)
        {
            case ManagerType.SFX:
                //base.DrawEditor(enumIndex);
                drawSoundItem.SetSelected(this.MenuTree.Selection.SelectedValue);//获取选中的ScriptableObject Item
                break;
            case ManagerType.VFX:
                //base.DrawEditor(enumIndex);//按照index来设置选中Item
                drawLevelItem.SetSelected(this.MenuTree.Selection.SelectedValue);//获取选中的ScriptableObject Item
                //base.DrawEditor(enumIndex);//按照index来设置选中Item
                break;
            case ManagerType.Extra:
                //暂时还没有设置可选项
                //base.DrawEditor(enumIndex);//按照index来设置选中Item
                break;
            default:
                break;
        }
        DrawEditor((int)managerType);
    }

    //获得需要描绘的对象
    //需要有这个方法才会出现managerType的SFX VFX等选项(override的都是odin本身自带的方法，功能见官方注释)
    protected override IEnumerable<object> GetTargets()//????但很好奇他怎么知道enumIndex对应的是哪个枚举类型？
    {
        Debug.Log("GetTargets获取选择对象!!");
        List<object> targets = new List<object>();

        //按枚举中顺序来Add

        //targets.Add(drawSoundItem);//ScriptableObject版本
        targets.Add(soundManagerPanel);//场景中mono单例版本

        targets.Add(drawLevelItem);
        targets.Add(null);//没实现的模块用null代替（暂时对应Extra）不加会导致 draw出错或 显示 默认的自定义区块的东西
        targets.Add(base.GetTarget());

        enumIndex = targets.Count - 1;//为什么要-1 ?
        return targets;
    }

    #endregion


    #region 自定义(区块)成员
    //需调节对象面板分类
    public enum ManagerType
    {
        SFX,
        VFX,
        Extra
    }

    [LabelText("面板分区")]
    [LabelWidth(80f)]
    [ShowInInspector]
    //枚举的标签选一个用就好//[EnumPaging]//分页显示枚举
    [EnumToggleButtons]//显示所有枚举
    [OnValueChanged("TypeChange")]//""里是成员方法名,managerType值发生变化时调用
    private ManagerType managerType;
    private int enumIndex = 0;
    private bool CanTreeRebuild = false;
    //在选择不同的面板时会调用 GetTargets()方法  改变managerType的值
    private void TypeChange()//好像功能和Odin的DrawMenu有点冲突
    {
        CanTreeRebuild = true;
        Debug.Log($"TypeChange!!! 菜单树重建情况{CanTreeRebuild}");
        switch (managerType)
        {
            case ManagerType.SFX:
                break;
            case ManagerType.VFX:
                break;
            case ManagerType.Extra:
                break;
            default:
                break;
        }
    }



    //ScriptableObject Part
    //[ShowInInspector]
    [PropertyOrder(-1)]//这样可以确保置顶
    private OdinWindowItem<LevelConfig> drawLevelItem = new OdinWindowItem<LevelConfig>();

    //[ShowInInspector]
    [PropertyOrder(0)]//这样可以确保置顶
    private OdinWindowItem<SoundList> drawSoundItem = new OdinWindowItem<SoundList>();

    //SceneObject Part(在场景中的mono组件) 
    private SoundManagerRenderer soundManagerPanel = new SoundManagerRenderer();

    #endregion


    //--------------------------------------预览功能模块--------------------------------------------------

    //修饰特性时[HorizontalGroup][VerticalGroup][BoxGroup]最好只用一个
    //如果用了多个则会以对应特性形式 重复显示同一个字段
    [VerticalGroup("便利工具")]
    [FolderPath(RequireExistingPath = true)/*, BoxGroup("Asset路径读取处")*/]
    public string OutputPath;
    [VerticalGroup("便利工具"), LabelWidth(80), LabelText("预览")]
    [InlineEditor(InlineEditorModes.LargePreview)]
    //public Texture[] Preview;
    public List<Texture> Preview;//数组和List在序列化后没什么区别

    //[HorizontalGroup]
    [LabelWidth(100), LabelText("输入区"), TextArea]//LabelWidth标签到字段的距离
    public string InputField;

    //Button来弹出一个新的窗口，由这个窗口去处理该list对象
    public void ActionWindow()
    {
    }



}

