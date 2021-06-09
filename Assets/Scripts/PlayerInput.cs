using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerInput 
{
    private InputActionAsset inputActionAsset;
    private string inputActionAssetName => "Assets/AssetBundles_sai/Input/PlayerInput.inputactions";

    //按键
    public InputAction Move;
    public InputAction Lock;
    public InputAction Hook;
    public InputAction Jump;


    /// <summary>
    /// 初始化 输入事件
    /// </summary>
    public void InitInput() 
    {
        inputActionAsset = Object.Instantiate(GameAssetManager.LoadInputActionAsset(inputActionAssetName));

        //[0]好像是 面板里Action Maps栏的index 第一个的话对应的就是adventureMode
        InputActionMap inputActionMap = inputActionAsset.FindActionMap("AdventureMode");
        inputActionMap.Enable();
        //下面是 在建立 表现层和逻辑层的耦合
        //建立输入源和动作源之间的联系
        Move = inputActionMap.FindAction("Move");
        Move.Enable();
        Lock = inputActionMap.FindAction("Lock");
        Lock.Enable();
        Hook = inputActionMap.FindAction("Hook");
        Hook.Enable();
        Jump = inputActionMap.FindAction("Jump");
        Jump.Enable();
    }


}
