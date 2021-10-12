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
    public InputAction Interact;


    /// <summary>
    /// 初始化 输入事件
    /// </summary>
    public void InitInput()
    {
        inputActionAsset = Object.Instantiate(GameAssetManager.LoadInputActionAsset(inputActionAssetName));

        //[0]好像是 面板里Action Maps栏的index 第一个的话对应的就是adventureMode
        InputActionMap inputActionMap = inputActionAsset.FindActionMap("AdventureMode");
        inputActionMap.Enable();


        //相当于多了层抽象层，有需求可以修改不同的InPutActionMap输入源（比如像街机格斗就可以切换输入源InPutActionMap（player1 2 3 4）绑定到同一个角色上）
        //下面是 在建立 表现层和逻辑层的耦合
        //建立输入源和动作源之间的联系
        Move = inputActionMap.FindAction("Move"); 
        Lock = inputActionMap.FindAction("Lock");   
        Hook = inputActionMap.FindAction("Hook");      
        Jump = inputActionMap.FindAction("Jump");     
        Interact = inputActionMap.FindAction("Interact");
        EnableInput();
    }

    public void EnableInput()
    {
        Move.Enable();
        Lock.Enable();
        Hook.Enable();
        Jump.Enable();
        Interact.Enable();
        Debug.Log("激活所有按键");
    }

    public void DisableInput() 
    {
        Move.Disable();
        Lock.Disable();
        Hook.Disable();
        Jump.Disable();
        Interact.Disable();
        Debug.Log("禁用所有按键");
    }

}
