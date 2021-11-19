//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;
//using Sirenix.OdinInspector;
//using Sirenix.OdinInspector.Editor;
//using Sirenix.Utilities.Editor;
//using Sirenix.OdinInspector.Editor.Modules;


////注释掉的都是Part2教程的报废代码

////有点好奇这里面的方法都是由谁调用的
////教程地址https://www.youtube.com/watch?v=S29XkTlD9bw
////源码https://github.com/onewheelstudio/SirenixTutorialFiles/blob/master/THE%20Game%20Manager/TheGameManager.cs
//public class GoapManager : OdinMenuEditorWindow
//{
//    #region lesson1
 

//    //绘制空白窗口
//    [MenuItem("Sai_Framework/GoapManager")]
//    public static void OpenWindow()
//    {
//        GetWindow<GoapManager>().Show();
//    }

    
//    //绘制top bar 功能模块
//    public enum FunctionModule
//    {
//        Goal,
//        Action,
//        Condition,
//        Planning,
//    }

//    [LabelText("Module View")]
//    [LabelWidth(80f)]
//    [EnumToggleButtons]
//    [ShowInInspector]
//    private FunctionModule m_functionModule;
//    private int enumIndex = 0;


//    //private GoapItem<ModuleData> m_drawModules = new GoapItem<ModuleData>();
//    //private GoapItem<ColorData> m_drawColors = new GoapItem<ColorData>();


//    private string modulePath => "";

//    //不实现这个抽象成员会报错
//    protected override OdinMenuTree BuildMenuTree()
//    {
//        OdinMenuTree tree = new OdinMenuTree();

//        //switch (m_functionModule)
//        //{
//        //    case FunctionModule.Goal:
//        //            tree.AddAllAssetsAtPath("Module Data", modulePath, typeof(ModuleData));
//        //            break;
//        //    default:
//        //        break;
//        //}

//        return tree;
//    }

//    protected override void Initialize()
//    {
        
//    }


//    protected override IEnumerable<object> GetTargets()
//    {
//        List<object> targets = new List<object>();
//        targets.Add(base.GetTarget());

//        enumIndex = targets.Count - 1;

//        return targets;
//    }

//    protected override void OnGUI()
//    {
//        SirenixEditorGUI.Title("Goap编辑器", "使用注释待更新", TextAlignment.Center, true);

//        switch (m_functionModule)
//        {
//            case FunctionModule.Goal:
//            case FunctionModule.Condition:
//            case FunctionModule.Action:
//            case FunctionModule.Planning:
//                //DrawEditor(0);
//                break;
//            default:
//                break;
//        }

//        base.OnGUI();
//    }

//    #endregion

//    protected override void DrawMenu()
//    {
//        switch (m_functionModule)
//        {
//            case FunctionModule.Goal:
//            case FunctionModule.Condition:
//            case FunctionModule.Action:
//            case FunctionModule.Planning:
//                base.DrawMenu();
//                break;
//            default:
//                break;
//        }
//    }



//    protected override void DrawEditors()
//    {
//        switch (m_functionModule)
//        {
//            case FunctionModule.Goal:
//                DrawEditor(0);
//                break;
//            case FunctionModule.Condition:
//                DrawEditor(0);
//                break;
//            case FunctionModule.Action:
//                DrawEditor(0);
//                break;
//            case FunctionModule.Planning:
//                DrawEditor(0);
//                break;
//            default:
//                break;
//        }
//    }

//}



//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;
//using Sirenix.OdinInspector;
//using Sirenix.OdinInspector.Editor;
//using Sirenix.Utilities.Editor;

//public class TheGameManager : OdinMenuEditorWindow
//{
//    [OnValueChanged("StateChange")]
//    [LabelText("Manager View")]
//    [LabelWidth(100f)]
//    [EnumToggleButtons]
//    [ShowInInspector]
//    private ManagerState managerState;
//    private int enumIndex = 0;
//    private bool treeRebuild = false;

//    private DrawUniverse drawUniverse = new DrawUniverse();
//    private DrawNPC drawNPC = new DrawNPC();
//    private DrawSFX drawSFX = new DrawSFX();

//    private DrawSelected<ModuleData> drawModules = new DrawSelected<ModuleData>();
//    private DrawSelected<ColorData> drawColors = new DrawSelected<ColorData>();
//    private DrawSelected<OpenInventory.ItemData> drawItems = new DrawSelected<OpenInventory.ItemData>();
//    private DrawSelected<RecipeBase> drawRecipes = new DrawSelected<RecipeBase>();

//    //paths to SOs in project
//    private string modulePath = "Assets/Prefabs/Ships/Module/ModuleData";
//    private string colorPath = "Assets/Resources/ColorData";
//    private string itemPath = "Assets/Resources/Items";
//    private string recipePath = "Assets/Scripts/Industry/Recipe Data";

//    [MenuItem("Tools/The Game Manager")]
//    public static void OpenWindow()
//    {
//        GetWindow<TheGameManager>().Show();
//    }

//    private void StateChange()
//    {
//        treeRebuild = true;
//    }

//    protected override void Initialize()
//    {
//        drawModules.SetPath(modulePath);
//        drawColors.SetPath(colorPath);
//        drawItems.SetPath(itemPath);
//        drawRecipes.SetPath(recipePath);

//        drawUniverse.FindMyObject();
//        drawNPC.FindMyObject();
//        drawSFX.FindMyObject();
//    }

//    protected override void OnGUI()
//    {
//        if (treeRebuild && Event.current.type == EventType.Layout)
//        {
//            ForceMenuTreeRebuild();
//            treeRebuild = false;
//        }

//        SirenixEditorGUI.Title("The Game Manager", "Because every hobby game is overscoped", TextAlignment.Center, true);
//        EditorGUILayout.Space();

//        switch (managerState)
//        {

//            case ManagerState.modules:
//            case ManagerState.items:
//            case ManagerState.recipes:
//            case ManagerState.color:
//                DrawEditor(enumIndex);
//                break;
//            default:
//                break;
//        }
//        EditorGUILayout.Space();

//        base.OnGUI();
//    }

//    protected override void DrawEditors()
//    {
//        switch (managerState)
//        {
//            case ManagerState.universe:
//                DrawEditor(enumIndex);
//                break;
//            case ManagerState.modules:
//                drawModules.SetSelected(this.MenuTree.Selection.SelectedValue);
//                break;
//            case ManagerState.npc:
//                DrawEditor(enumIndex);
//                break;
//            case ManagerState.items:
//                drawItems.SetSelected(this.MenuTree.Selection.SelectedValue);
//                break;
//            case ManagerState.recipes:
//                drawRecipes.SetSelected(this.MenuTree.Selection.SelectedValue);
//                break;
//            case ManagerState.color:
//                drawColors.SetSelected(this.MenuTree.Selection.SelectedValue);
//                break;
//            case ManagerState.sfx:
//                DrawEditor(enumIndex);
//                break;
//            default:
//                break;
//        }

//        DrawEditor((int)managerState);
//    }

//    protected override IEnumerable<object> GetTargets()
//    {
//        List<object> targets = new List<object>();
//        targets.Add(drawUniverse);
//        targets.Add(drawModules);
//        targets.Add(drawNPC);
//        targets.Add(drawItems);
//        targets.Add(drawRecipes);
//        targets.Add(drawColors);
//        targets.Add(drawSFX);
//        targets.Add(base.GetTarget());

//        enumIndex = targets.Count - 1;

//        return targets;
//    }

//    protected override void DrawMenu()
//    {
//        switch (managerState)
//        {

//            case ManagerState.modules:
//            case ManagerState.items:
//            case ManagerState.recipes:
//            case ManagerState.color:
//                base.DrawMenu();
//                break;
//            default:
//                break;
//        }
//    }

//    protected override OdinMenuTree BuildMenuTree()
//    {
//        OdinMenuTree tree = new OdinMenuTree();

//        switch (managerState)
//        {

//            case ManagerState.modules:
//                tree.AddAllAssetsAtPath("Module Data", modulePath, typeof(ModuleData));
//                break;
//            case ManagerState.items:
//                tree.AddAllAssetsAtPath("Item Data", itemPath, typeof(OpenInventory.ItemData));
//                break;
//            case ManagerState.recipes:
//                tree.AddAllAssetsAtPath("Recipes", recipePath, typeof(RecipeBase));
//                break;
//            case ManagerState.color:
//                tree.AddAllAssetsAtPath("Color Data", colorPath, typeof(ColorData));
//                break;
//            default:
//                break;
//        }

//        return tree;
//    }

//    public enum ManagerState
//    {
//        universe,
//        modules,
//        npc,
//        items,
//        recipes,
//        color,
//        sfx
//    }
//}

//public class DrawSelected<T> where T : ScriptableObject
//{
//    [InlineEditor(InlineEditorObjectFieldModes.CompletelyHidden)]
//    public T selected;

//    [LabelWidth(100)]
//    [PropertyOrder(-1)]
//    [ColorGroupAttribute("CreateNew", 1f, 1f, 1f)]
//    [HorizontalGroup("CreateNew/Horizontal")]
//    public string nameForNew;

//    private string path;

//    [HorizontalGroup("CreateNew/Horizontal")]
//    [GUIColor(0.7f, 0.7f, 1f)]
//    [Button]
//    public void CreateNew()
//    {
//        if (nameForNew == "")
//            return;

//        T newItem = ScriptableObject.CreateInstance<T>();
//        //newItem.name = "New " + typeof(T).ToString();

//        if (path == "")
//            path = "Assets/";

//        AssetDatabase.CreateAsset(newItem, path + "\\" + nameForNew + ".asset");
//        AssetDatabase.SaveAssets();

//        nameForNew = "";
//    }

//    [HorizontalGroup("CreateNew/Horizontal")]
//    [GUIColor(1f, 0.7f, 0.7f)]
//    [Button]
//    public void DeleteSelected()
//    {
//        if (selected != null)
//        {
//            string _path = AssetDatabase.GetAssetPath(selected);
//            AssetDatabase.DeleteAsset(_path);
//            AssetDatabase.SaveAssets();
//        }
//    }

//    public void SetSelected(object item)
//    {
//        var attempt = item as T;
//        if (attempt != null)
//            this.selected = attempt;
//    }

//    public void SetPath(string path)
//    {
//        this.path = path;
//    }
//}



//public class DrawUniverse : SceneObject<UniverseCreator>
//{
//    [ShowIf("@myObject != null")]
//    [GUIColor(0.7f, 1f, 1f)]
//    [ButtonGroup("Top Button")]
//    private void SomeUniverseFunction1()
//    {

//    }

//    [ShowIf("@myObject != null")]
//    [GUIColor(0.7f, 0.7f, 1f)]
//    [ButtonGroup("Top Button")]
//    private void SomeUniverseFunction2()
//    {

//    }
//}

//public class DrawNPC : SceneObject<NPCManager>
//{

//}

//public class DrawSFX : SceneObject<SFXManager>
//{

//}