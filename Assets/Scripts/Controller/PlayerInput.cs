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
    public InputAction ChangeWeaponLeft;
    public InputAction ChangeWeaponRight;


    /// <summary>
    /// 初始化 输入事件
    /// </summary>
    public void InitInput()
    {
        inputActionAsset = Object.Instantiate(GameAssetManager.LoadInputActionAsset(inputActionAssetName));

        //[0]好像是 面板里Action Maps栏的index 第一个的话对应的就是adventureMode
        InputActionMap AdventureMap = inputActionAsset.FindActionMap("Adventure");
        AdventureMap.Enable();
        InputActionMap UIMap = inputActionAsset.FindActionMap("UI");
        AdventureMap.Disable();//切换到UI交互模式再激活

        //相当于多了层抽象层，有需求可以修改不同的InPutActionMap输入源（比如像街机格斗就可以切换输入源InPutActionMap（player1 2 3 4）绑定到同一个角色上）
        //下面是 在建立 表现层和逻辑层的耦合
        //建立输入源和动作源之间的联系
        Move = AdventureMap.FindAction("Move"); 
        Lock = AdventureMap.FindAction("Lock");   
        Hook = AdventureMap.FindAction("Hook");      
        Jump = AdventureMap.FindAction("Jump");     
        Interact = AdventureMap.FindAction("Interact");
        ChangeWeaponLeft = AdventureMap.FindAction("ChangeWeaponLeft");
        ChangeWeaponRight = AdventureMap.FindAction("ChangeWeaponRight");
        EnableInput();
    }

    public void EnableInput()
    {
        Move.Enable();
        Lock.Enable();
        Hook.Enable();
        Jump.Enable();
        Interact.Enable();
        ChangeWeaponLeft.Enable();
        ChangeWeaponRight.Enable();
        Debug.Log("激活所有按键");
    }

    public void DisableInput() 
    {
        Move.Disable();
        Lock.Disable();
        Hook.Disable();
        Jump.Disable();
        Interact.Disable();
        ChangeWeaponLeft.Disable();
        ChangeWeaponRight.Disable();
        Debug.Log("禁用所有按键");
    }

}
