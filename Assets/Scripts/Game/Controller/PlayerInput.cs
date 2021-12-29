using UnityEngine.InputSystem;
using UnityEngine;
using System.Collections.Generic;


public class PlayerInput 
{
    protected InputActionAsset inputActionAsset;
    protected string inputActionAssetName => "Assets/AssetBundles_sai/Input/PlayerInput.inputactions";
    public List<InputAction> PlayModeInputActions = new List<InputAction>();
    public List<InputAction> UIModeInputActions = new List<InputAction>();

    //基础操作按键
    public InputAction Move;
    public InputAction Jump;

    public virtual void InitInput() 
    {
        inputActionAsset = Object.Instantiate(GameAssetManager.LoadInputActionAsset(inputActionAssetName));
    }
}


public class AdvPlayerInput:PlayerInput
{
    //按键
    public InputAction Lock;
    public InputAction Hook;
    public InputAction Interact;
    public InputAction ChangeWeaponLeft;
    public InputAction ChangeWeaponRight;
    public InputAction Attack;

    /// <summary>
    /// 初始化 输入事件
    /// </summary>
    public override void InitInput()
    {
        base.InitInput();
        //[0]好像是 面板里Action Maps栏的index 第一个的话对应的就是adventureMode
        InputActionMap AdventureMap = inputActionAsset.FindActionMap("Adventure");
        AdventureMap.Enable();
        InputActionMap UIMap = inputActionAsset.FindActionMap("UI");
        UIMap.Disable();//切换到UI交互模式再激活

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
        Attack = AdventureMap.FindAction("Attack");
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
        Attack.Enable();
        Debug.Log("激活adv所有按键");
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
        Attack.Disable();
        Debug.Log("禁用adv所有按键");
    }
}

public class BtlPlayerInput:PlayerInput
{
    //不知道Move可不可以 包含 下蹲 和上跳
    public InputAction LightPunch,MediumPunch,LightKick,MediumKick;


    public override void InitInput()
    {
        base.InitInput();
        InputActionMap BattleMap = inputActionAsset.FindActionMap("Battle");
        BattleMap.Enable();
        //InputActionMap UIMap = inputActionAsset.FindActionMap("UI");
        //UIMap.Disable();//切换到UI交互模式再激活

        Move = BattleMap.FindAction(nameof(Move)); //前提：inputAction的Asset名和字段名同名
        PlayModeInputActions.Add(Move);
        Jump = BattleMap.FindAction(nameof(Jump));
        PlayModeInputActions.Add(Jump);
        LightPunch = BattleMap.FindAction(nameof(LightPunch));
        PlayModeInputActions.Add(LightPunch);

        EnableInput();
    }

    public void EnableInput()
    {
        foreach (var InputAction in PlayModeInputActions)
        {
            InputAction.Enable();
        }
        Debug.Log("激活btl所有按键");
    }

    public void DisableInput()
    {
        foreach (var InputAction in PlayModeInputActions)
        {
            InputAction.Disable();
        }
        Debug.Log("禁用btl所有按键");
    }
}