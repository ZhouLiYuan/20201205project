using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using Sirenix.OdinInspector.Editor.Modules;


//注释掉的都是Part2教程的报废代码

//有点好奇这里面的方法都是由谁调用的
//教程地址https://www.youtube.com/watch?v=S29XkTlD9bw
//源码https://github.com/onewheelstudio/SirenixTutorialFiles/blob/master/THE%20Game%20Manager/TheGameManager.cs
public class GoapManager : OdinMenuEditorWindow
{
    #region lesson1
 

    //绘制空白窗口
    [MenuItem("Sai_Framework/GoapManager")]
    public static void OpenWindow()
    {
        GetWindow<GoapManager>().Show();
    }

    
    //绘制top bar 功能模块
    public enum FunctionModule
    {
        Goal,
        Action,
        Condition,
        Planning,
    }

    [LabelText("Module View")]
    [LabelWidth(80f)]
    [EnumToggleButtons]
    [ShowInInspector]
    private FunctionModule m_functionModule;
    private int enumIndex = 0;


    //private GoapItem<ModuleData> m_drawModules = new GoapItem<ModuleData>();
    //private GoapItem<ColorData> m_drawColors = new GoapItem<ColorData>();


    private string modulePath => "";

    //不实现这个抽象成员会报错
    protected override OdinMenuTree BuildMenuTree()
    {
        OdinMenuTree tree = new OdinMenuTree();

        //switch (m_functionModule)
        //{
        //    case FunctionModule.Goal:
        //            tree.AddAllAssetsAtPath("Module Data", modulePath, typeof(ModuleData));
        //            break;
        //    default:
        //        break;
        //}

        return tree;
    }

    protected override void Initialize()
    {
        
    }


    protected override IEnumerable<object> GetTargets()
    {
        List<object> targets = new List<object>();
        targets.Add(base.GetTarget());

        enumIndex = targets.Count - 1;

        return targets;
    }

    protected override void OnGUI()
    {
        SirenixEditorGUI.Title("Goap编辑器", "使用注释待更新", TextAlignment.Center, true);

        switch (m_functionModule)
        {
            case FunctionModule.Goal:
            case FunctionModule.Condition:
            case FunctionModule.Action:
            case FunctionModule.Planning:
                DrawEditor(0);
                break;
            default:
                break;
        }

        base.OnGUI();
    }

    #endregion

    protected override void DrawMenu()
    {
        switch (m_functionModule)
        {
            case FunctionModule.Goal:
            case FunctionModule.Condition:
            case FunctionModule.Action:
            case FunctionModule.Planning:
                base.DrawMenu();
                break;
            default:
                break;
        }
    }



    protected override void DrawEditors()
    {
        switch (m_functionModule)
        {
            case FunctionModule.Goal:
                DrawEditor(0);
                break;
            case FunctionModule.Condition:
                DrawEditor(0);
                break;
            case FunctionModule.Action:
                DrawEditor(0);
                break;
            case FunctionModule.Planning:
                DrawEditor(0);
                break;
            default:
                break;
        }
    }

}
