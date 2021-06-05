using UnityEngine;
using System;

public class PlayerRole : Entity
{
    private Updater updater;
    private FSM hookFsm;
    private FSM moveFsm;

    Transform ch_transform;
    Rigidbody2D ch_rigidbody2d;

    //重力
    private float maxGravity = -10f;
    private float gravity = 9.8f;
    public bool canApplyGravity = true;

    public bool canMoveHorizontal = true;

    //刚体速度
    Rigidbody2D rg2d;
    public Vector2 Velocity
    {
        get { return rg2d.velocity; }
        set { rg2d.velocity = value; }
    }

    //地面检测
    public GroundDetect GroundDetect { get; private set; }

    //输入 一些trigger
    public PlayerInput playerInput;
    public Vector2 inputAxis;
    public bool IsLockPressed { get; private set; }
    public bool IsHookPressed { get; private set; }
    public bool IsJumpPressed { get; private set; }

    public event Action<GameObject> OnShowLockTarget;
    public void LockTarget(GameObject target) { OnShowLockTarget?.Invoke(target); }

    /// <summary>
    ///初始化脚本实例字段（建立逻辑层和表现层联系） 
    /// </summary>
    public PlayerRole(GameObject roleGobj) : base(roleGobj)
    {
        ch_rigidbody2d = roleGobj.GetComponent<Rigidbody2D>();
        ch_transform = roleGobj.GetComponent<Transform>();
        GroundDetect = roleGobj.GetComponentInChildren<GroundDetect>();
        
        //updater相关  
    }

    /// <summary>
    /// 按键绑定到物理输入
    /// </summary>
    /// <param name="inputHandler"></param>
    public void BindInput(PlayerInput inputHandler) 
    {
        this.playerInput = inputHandler;
        //move
        inputHandler.Move.performed += context => inputAxis = context.ReadValue<Vector2>();
        inputHandler.Move.started += context => inputAxis = context.ReadValue<Vector2>();
        inputHandler.Move.canceled += context => inputAxis = Vector2.zero;

        //trigger (判断按键有无按下)
        inputHandler.Lock.started += context => IsLockPressed = true;
        inputHandler.Lock.canceled += context => IsLockPressed =false;

        inputHandler.Hook.started += context => IsHookPressed = true;
        inputHandler.Hook.canceled += context => IsHookPressed = false;

        inputHandler.Jump.started += context => IsJumpPressed = true;
        inputHandler.Jump.canceled += context => IsJumpPressed = false;
    }


    /// <summary>
    ///  //配置每个功能的FSM，为每个状态传owner实例
    /// </summary>
    private void InitFSM() 
    {
        hookFsm = new FSM();
        hookFsm.AddState<IdleState>().SetPlayerRole(this);
        hookFsm.AddState<LockState>().SetPlayerRole(this);
        hookFsm.AddState<MoveToTargetState>().SetPlayerRole(this);

        moveFsm = new FSM();
        moveFsm.AddState<MoveState>().SetPlayerRole(this);
    }

    private void OnUpdate(float deltaTime) 
    {
        hookFsm.Update(deltaTime);
        moveFsm.Update(deltaTime);
    }

    //物理相关的刷新
    private void OnFixedUpdate(float fixedDeltaTime)
    {
        hookFsm.FixedUpdate(fixedDeltaTime);
        moveFsm.FixedUpdate(fixedDeltaTime);
        if (canApplyGravity) ApplyGravity(fixedDeltaTime);
    }

    private void ApplyGravity(float fixedDeltaTime) 
    {
        //Mathf.Max返回两个指定数字中较大
        Velocity = new Vector2(Velocity.x, Mathf.Max(Velocity.y - fixedDeltaTime * gravity, maxGravity));
    }

    /////// <summary>
    ///////角色反转功能
    ///////使用前提，动画中没有k scale或者rotation的帧
    ///////当Gobj scale和速度方向相反的时候反转
    /////// </summary>
    ////private void GraphicFlip()
    ////{
    ////    //先判断非空
    ////    if (m_Role)
    ////    {
    ////        if (move.x > 0.01f && m_Role.transform.localScale.x == -1)
    ////        {
    ////            m_Role.transform.localScale = new Vector3(1, ch_transform.localScale.y, ch_transform.localScale.z);
    ////        }
    ////        else if (move.x < -0.01f && m_Role.transform.localScale.x == 1)
    ////        {
    ////            m_Role.transform.localScale = new Vector3(-1, ch_transform.localScale.y, ch_transform.localScale.z);
    ////        }
    ////    }
    ////}


    //private void Update()
    //{

    //    //在自己类体里调用就不用声明实例
    //    HandleInput();
    //    GraphicFlip();

    //}
    //private void HandleInput()
    //{
    //    //移动
    //    move.x = Input.GetAxis("Horizontal");
    //    HorizontalMovement();
    //    //跳跃
    //    if (Input.GetKeyDown(KeyCode.Space)) { Jump(); }
    //    //钩锁
    //    if (Input.GetKey(KeyCode.G))
    //    {
    //        Hook();
    //    }
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


