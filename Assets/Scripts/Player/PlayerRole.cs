using UnityEngine;
using System.Collections.Generic;
using System;

public class PlayerRole : Entity
{
    public string RoleName => PlayerManager.m_RoleName;

    private Updater updater;

    //状态机分层
    private FSM hookFsm;
    private FSM generalFsm;


    //需要序列化动态调节
    public int maxHP = 1000;
    private int hp;
    public int HP
    {
        get { return hp; }
        set
        {
            if (0 <= value && value <= maxHP) { hp = value; }
            else if (value < 0) { hp = 0; }
            else { hp = maxHP; }
        }
    }
    //重力
    /// <summary>
    /// 代指y轴速度最大可加速到maxGravity 
    /// </summary>
    private float maxGravity = -10f;
    private float gravity = 9.8f;

    //自身属性

    //物理
    public float jumpSpeed = 8f;
    public float moveSpeed = 5f;

    public int money;

    //层级
    public Transform topNodeTransform;
    public Transform animatorTransform;
    public Animator animator;

    Rigidbody2D rg2dtest;//临时工
    Rigidbody2D rg2d;
    public Vector2 Velocity
    {
        get { return rg2d.velocity; }
        set { rg2d.velocity = value; }
    } //刚体速度



    public GroundDetect GroundDetect { get; private set; } //地面检测
    public Collider2D HitCollider; // 受击框

    //输入trigger
    public PlayerInput playerInput;
    public Vector2 inputAxis;
    public bool IsLockPressed { get; private set; }
    public bool IsHookPressed { get; private set; }
    public bool IsInteractPressed { get; private set; }
    public bool IsJumpPressed { get; private set; }
    public bool IsJumpTriggered { get; private set; }

    //逻辑trigger
    public bool canApplyGravity = true;
    public bool canMoveHorizontal = true;

    //damage相关
    public float invincibleInterval = 1f;
    private float invincibleTime;
    public float InvincibleTime
    {
        get { return invincibleTime; }
        set
        {
            if (0 <= value) { invincibleTime = value; }
            else if (value < 0) { invincibleTime = 0; }
        }
    }
    public bool isAttacked = false;

    //交互相关
    public bool isInInteractArea => GobjsInInteractArea != null;
    public bool IsInteracting = false;//当前不在交互状态才能和其他对象交互
    public List<GameObject> GobjsInInteractArea = new List<GameObject>();
    public GameObject nearestInteractableGobj;
    public NPC currentInteractingNPC;
    //public InteractableType currentInteractingType;
    //public InteractableData interactingData;

    public string currentPlaceName;//角色当前所处的位置


    public BaseWeapon currentWeapon;  //当前装备的武器
    public List<BaseWeapon> availableWeapons = new List<BaseWeapon>();  //因为一般也不会用string去查找和切换武器，所以用List直接用index顺序查找


    public void OnEnterInteractArea(GameObject target)
    {
        Debug.Log($"进入{target.name}的交互区域");
        GobjsInInteractArea.Add(target);
    }

    public void Interact()
    {
        if (nearestInteractableGobj == null) return;
        IsInteracting = true;
        switch (nearestInteractableGobj.tag)
        {
            case "Collectable":
                //不用打开面板
                break;
            case "Item":
                var item = ItemManager.nameDic[nearestInteractableGobj.name];
                break;
            case "NPC":
                //currentInteractingType = InteractableType.NPC;
                currentInteractingNPC = NPCManager.nameDic[nearestInteractableGobj.name];
                StoryManager.InteractingNPCName = currentInteractingNPC.UniqueName;
                //通知 NPCInteractablePanel持有者 进入交互状态
                currentInteractingNPC.isInteractingWithPlayer = true;//trigger
                break;
            default:
                Debug.Log("无法判断交互对象tag");
                break;
        }
        //禁用交互键以外的所有按键
        playerInput.DisableInput();
        playerInput.Interact.Enable();
    }

    public void ExitInteract()
    {
        //一切Interact有关的都需要被重置
        IsInteracting = false;
        currentInteractingNPC = null;
        //恢复所有按键
        playerInput.EnableInput();
    }

    //离开交互区域 和 结束交互 是两回事
    public void ExitInteractArea(GameObject target)
    {
        Debug.Log($"离开{target.name}的交互区域");
        GobjsInInteractArea.Remove(target);
    }

    //和Player声明周期相关的 Player内部event
    public event Action<GameObject> OnShowLockTarget;
    public void LockTarget(GameObject target) { OnShowLockTarget?.Invoke(target); }

    public event Func<DamageData> OnAttacked;
    public void GetDamage()
    {
        var data = OnAttacked?.Invoke();
        float finalDamageValue = DamageSystem.CalculateDamage(data);
        HP -= (int)finalDamageValue;
    }



    /// <summary>
    ///初始化脚本实例字段（建立逻辑层和表现层联系） 
    /// </summary>
    public PlayerRole(GameObject roleGobj) : base(roleGobj)
    {
        hp = maxHP;
        rg2d = roleGobj.GetComponent<Rigidbody2D>();

        topNodeTransform = roleGobj.transform;
        animator = Find<Animator>("animator_top");
        animatorTransform = animator.transform;
        rg2dtest = animator.transform.GetComponent<Rigidbody2D>();


        //Transform = roleGobj.GetComponent<Transform>();
        GroundDetect = roleGobj.GetComponentInChildren<GroundDetect>();

        //updater相关  
        //为场景中叫Updater的Gobj添加逻辑层Updater组件
        updater = Updater.AddUpdater(roleGobj);
        //单一方法 作为 一个Action参数传入Action集合（之后再集中调用）
        updater.AddUpdateFunction(OnUpdate);
        updater.AddFixedUpdateFunction(OnFixedUpdate);

        InitFSM();
    }

    /// <summary>
    /// 安排每个Action会回调的方法（），具体修改rigidbody velocity之类的应该放在对应的State类里
    /// </summary>
    /// <param name="inputHandler"></param>
    public void BindInput(PlayerInput inputHandler)
    {
        //对照
        //按下GetKeyDown----Performed
        //按住GetKey----Started(按住包含了按下瞬间Performed）
        //松开GetKeyUp----Canceled


        //换角色时 解绑输入需要用到playerInput 这个引用缓存
        this.playerInput = inputHandler;
        //move(实际物理输入的值) 
        playerInput.Move.performed += context => inputAxis = context.ReadValue<Vector2>();
        playerInput.Move.started += context => inputAxis = context.ReadValue<Vector2>();
        playerInput.Move.canceled += context => inputAxis = Vector2.zero;

        //trigger (判断按键有无按下)
        playerInput.Jump.performed += context => IsJumpTriggered = true;
        playerInput.Jump.started += context => IsJumpPressed = true;
        playerInput.Jump.canceled += context => IsJumpPressed = false;

        playerInput.Lock.started += context => IsLockPressed = true;
        playerInput.Lock.canceled += context => IsLockPressed = false;

        playerInput.Hook.started += context => IsHookPressed = true;
        playerInput.Hook.canceled += context => IsHookPressed = false;

        playerInput.Interact.performed += context => IsInteractPressed = true;
        //playerInput.Interact.started += context => IsInteractPressed = true;
        playerInput.Interact.canceled += context => IsInteractPressed = false;

    }



    /// <summary>
    ///  //配置每个功能的FSM，为每个状态传owner实例
    /// </summary>
    private void InitFSM()
    {
        hookFsm = new HookFSM();
        hookFsm.AddState<IdleState>().SetPlayerRole(this);
        hookFsm.AddState<LockState>().SetPlayerRole(this);
        hookFsm.AddState<MoveToTargetState>().SetPlayerRole(this);

        //临时工，待删
        hookFsm.AddState<InteractState>().SetPlayerRole(this);

        //分不同的FSM其实就是分层处理
        generalFsm = new GeneralFSM();
        generalFsm.AddState<IdleState>().SetPlayerRole(this);
        generalFsm.AddState<MoveState>().SetPlayerRole(this);
        generalFsm.AddState<JumpState>().SetPlayerRole(this);

        generalFsm.AddState<DamagedState>().SetPlayerRole(this);
        generalFsm.AddState<InteractState>().SetPlayerRole(this);



    }

    private void OnUpdate(float deltaTime)
    {
        IsJumpTriggered = playerInput.Jump.triggered;
        generalFsm.Update(deltaTime);
        hookFsm.Update(deltaTime);


        if (isInInteractArea)
        {
            nearestInteractableGobj = SceneObjManager.GetNearest(topNodeTransform.position, GobjsInInteractArea);
        }

        //不是引用所以必须放在Update里实时更新（rg2dtest.velocity会有些许滞后）
        rg2dtest.velocity = rg2d.velocity;
        //修复滞后
        rg2dtest.position = rg2d.position;
    }

    //物理相关的刷新
    private void OnFixedUpdate(float fixedDeltaTime)
    {
        generalFsm.FixedUpdate(fixedDeltaTime);
        hookFsm.FixedUpdate(fixedDeltaTime);

        if (canApplyGravity) ApplyGravity(fixedDeltaTime);
    }

    private void ApplyGravity(float fixedDeltaTime)
    {
        //Mathf.Max返回两个指定数字中较大
        Velocity = new Vector2(Velocity.x, Mathf.Max(Velocity.y - fixedDeltaTime * gravity, maxGravity));
    }

    private void Collect()
    {

    }


    //public T Find<T>(string path) where T : UnityEngine.Object
    //{
    //    var t = topNodeTransform.Find(path);
    //    if (typeof(T) == typeof(Transform)) return t as T;
    //    if (typeof(T) == typeof(GameObject)) return t.gameObject as T;
    //    return t.GetComponent<T>();
    //}

    //public void HorizontalMovement()
    //{
    //    Debug.Log($"水平输入 = {move.x}");
    //    ch_rigidbody2d.velocity = new Vector2(move.x * maxSpeed, ch_rigidbody2d.velocity.y);
    //}


    ///// <summary>
    ///// 一次跳跃，2段跳，浮空跳
    ///// </summary>
    //private void Jump()

    //{
    //    if (IsGrounded)
    //    {
    //        jumpCount = 2;
    //        ch_rigidbody2d.velocity = new Vector2(ch_rigidbody2d.velocity.x, jumpSpeed);
    //        jumpCount--;
    //        Debug.Log($"剩余跳跃次数{jumpCount}");
    //    }
    //    //
    //    else if (Input.GetKeyDown(KeyCode.Space) && !IsGrounded && jumpCount > 0)
    //    {
    //        ch_rigidbody2d.velocity = new Vector2(ch_rigidbody2d.velocity.x, jumpSpeed);
    //        jumpCount = 0;
    //    }
    //}


    //private void Hook()
    //{

    //    var worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //    //所挂载Gobj坐标 到 鼠标坐标(实现实时变化的距离)
    //    var direction = worldPosition - m_Role.transform.position;
    //    Debug.DrawLine(m_Role.transform.position, worldPosition, Color.green);

    //    //防止射线撞到挂载Gobj本身，需要设置忽略层,参数列表再追加多一个LayerMask
    //    var result = Physics2D.Raycast(m_Role.transform.position, direction, direction.magnitude, LayerMask.GetMask("Platform"));

    //    if (result.collider && result.collider.tag == "Platform")
    //    {
    //        Debug.Log("发射钩锁");
    //        m_hook.Shoot(result.point);
    //        //开按键才会飞过去
    //    }

    //}

    ////切换武器
    //public void ChangeWeapon(string weaponName)
    //{
    //    weapon = Resources.Load<GameObject>(weaponName);
    //    Debug.Log($"武器切换到{ weaponName}");
    //    weaponSelectCanvas.SetActive(false);
    //}

}


//[SerializeField] private Transform groundCheckPosition;
//private bool isGrounded;
///// <summary>
///// 旧的地面检测方法，现在用分离做成模块的脚本
///// </summary>
//private void CheckIsGrounded()
//{
//    var result = Physics2D.Raycast(groundCheckPosition.position, dir, distance);
//    //需要先判断result非空
//    if (result.collider)
//    {
//        isGrounded = result.collider.tag == "Platform";
//    }
//    //补足result为空的情况，因为result为空的时候不会执行上面的if语句， isGrounded会一直保留最开始时接地的判断true,会导致玩家可以无限跳
//    else { isGrounded = false; }

//    //Debug.Log($"{result.collider}{isGrounded}");
//    Debug.Log($"地面碰撞情况 = {isGrounded}");
//}

//private void OnDrawGizmos()
//{
//    Debug.DrawLine(groundCheckPosition.position, groundCheckPosition.position + transform.up * -1f * distance, Color.red, 1f);
//}


//代码素材库
/*
[SerializeField] private GameObject graphic;
[SerializeField] private Animator animator;
[SerializeField] private bool jumping;
[SerializeField] private AudioSource audio;
[SerializeField] private AudioClip[] stepSounds;
[SerializeField] private AudioClip[] jumpSounds;
*/



//音效功能

//     void Footstep()
//{
//    if (audio)
//    {
//        audio.PlayOneShot(stepSounds[Random.Range(0, stepSounds.Length)]);
//        Debug.Log("playFootstep");
//    }

//}

//void Jump()
//{
//    if (audio)
//    {
//        audio.PlayOneShot(jumpSounds[Random.Range(0, jumpSounds.Length)]);
//        Debug.Log("playjumpSounds");
//    }

//}

//void PlayAudio()
//{
//    if (Input.GetKeyDown(KeyCode.U))
//    {
//        var audioClip = Resources.Load<AudioClip>("footsteps2");
//        audio.PlayOneShot(audioClip);
//    }
//}

//private void Update()
//{

//    base.Update();
//    PlayAudio();
//    FindTransform();



//}


//添加特效到骨骼功能

//void FindTransform()
//{
//    GameObject fx = GameObject.Find("Particle1");

//    Transform head;
//    head = transform.Find("SkeletonUtility-SkeletonRoot/root/hip/torso/torso2/torso3/neck/head");
//    fx.transform.SetParent(head, true);
//}


