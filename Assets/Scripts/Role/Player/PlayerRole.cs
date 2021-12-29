using System;
using System.Collections.Generic;
using UnityEngine;

namespace Role.SelectableRole
{
    //注意：子类实现接口的方法，必须添加 Public关键字，否则报错
    public class PlayerRole : ControllableRole<AdvPlayerInput>/*, IPlayerRole*/
    {
        public string Name => PlayerManager.p1_RoleName;

        //状态机分层
        private FSM generalFsm;
        private FSM subFsm;

        //自身属性
        public int defValue;
        public int atkValue;

        public int money;

        //一些碰撞检测脚本
        public DamageReceiver DamageReceiver { get; private set; }

        //锁定相关
        public GameObject lockTarget = null;
        public Transform TargetTransform => lockTarget.transform;

        #region 输入trigger
        public bool IsLockPressed { get; protected set; }
        public bool IsHookPressed { get; protected set; }//按下
        public bool IsHookPressing { get; protected set; }//按住
        public bool IsInteractPressed { get; protected set; }
        public bool IsChangeWeaponLeftPressed { get; protected set; }
        public bool IsChangeWeaponRightPressed { get; protected set; }
        public bool IsAttackPressed { get; protected set; }
        #endregion


        #region Hook相关
        public Vector3 hookLocaloffsetPosSlash;
        public GameObject hookGobj;
        public float grapSpeed = 15f;//抓取敌人的速度
        public float hookSpeed = 15f;
        public float minDistance = 0.5f;    //触发最后向上速度 的 距离平台距离
        public float finalJumpSpeed = 5f;    //最后便于着陆的上升速度
        #endregion

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

        #region 交互相关
        public bool isInInteractArea => GobjsInInteractArea != null;
        public bool IsInteracting = false;//当前不在交互状态才能和其他对象交互
        public List<GameObject> GobjsInInteractArea = new List<GameObject>();
        public GameObject nearestInteractableGobj;
        public NPC currentInteractingNPC;
        //public InteractableType currentInteractingType;
        //public InteractableData interactingData;
        #endregion

        public string currentPlaceName;//角色当前所处的位置

        private int currentWeaponIndex = 0;
        private int CurrentWeaponIndex
        {
            get { return currentWeaponIndex; }
            set
            {
                //做成闭环(注意index从0开始 count从1开始)
                if (0 <= value && value <= availableWeapons.Count - 1) { currentWeaponIndex = value; }
                else if (value < 0) { currentWeaponIndex = availableWeapons.Count - 1; }
                else if (value > availableWeapons.Count - 1) { currentWeaponIndex = 0; }
                else Debug.Log("切换武器失败");
            }
        }
        public BaseWeapon currentWeapon;  //当前装备的武器
        public List<BaseWeapon> availableWeapons = new List<BaseWeapon>();  //因为一般不用string去查找和切换武器，用List直接用index顺序查找



        //---------------------------------------<方法>--------------------------------------------------

        #region 初始化

        //---------------------------------------<初始化>--------------------------------------------------

        /// <summary>
        ///初始化脚本实例字段（建立逻辑层和表现层联系） 
        /// </summary>
        public override void Init(GameObject roleGobj)
        {
            base.Init(roleGobj);


            hookGobj = Find<GameObject>("hook");
            hookLocaloffsetPosSlash = new Vector3(-0.1f, -0.2f, 0f);

            DamageReceiver = roleGobj.GetComponent<DamageReceiver>();
        }

        public void InitProperties(PlayerRoleConfig config)
        {
            base.InitProperties(config);
            atkValue = config.ATK;
            defValue = config.DEF;
            moveSpeed = config.MoveSpeed;
            jumpSpeed = config.JumpSpeed;
        }


        /// <summary>
        ///  //配置每个功能的FSM，为每个状态传owner实例
        /// </summary>
        protected override void InitFSM()
        {
            base.InitFSM();
            //每个FSM创建后记得关联OnUpdate()和OnFixUpdate()；

            //分不同的FSM其实就是分层处理
            generalFsm = new GeneralFSM();

            generalFsm.AddState<IdleState>().SetPlayerRole(this);
            generalFsm.AddState<MoveState>().SetPlayerRole(this);
            generalFsm.AddState<JumpState>().SetPlayerRole(this);

            generalFsm.AddState<DamagedState>().SetPlayerRole(this);
            generalFsm.AddState<InteractState>().SetPlayerRole(this);

            subFsm = new SubFSM();
            subFsm.AddState<PreSubActionState>().SetPlayerRole(this);

            subFsm.AddState<LockState>().SetPlayerRole(this);
            subFsm.AddState<HookToTargetState>().SetPlayerRole(this);
            subFsm.AddState<MoveToTargetState>().SetPlayerRole(this);
            subFsm.AddState<GetTargetState>().SetPlayerRole(this);

            subFsm.AddState<SwordAttackState>().SetPlayerRole(this);
            subFsm.AddState<PunchAttackState>().SetPlayerRole(this);
            subFsm.AddState<GunAttackState>().SetPlayerRole(this);

        }

        public void EquipWeapon(WeaponConfig weaponConfig)
        {
            if (currentWeapon != null) currentWeapon.GameObject.SetActive(false);
            var weapon = WeaponManager.SpawnPlayerWeapon(this, weaponConfig);
            //enemy武器切换成当前武器
            this.currentWeapon = weapon;
            currentWeapon.GameObject.SetActive(true);
        }

        /// <summary>
        /// 安排每个Action会回调的方法（），具体修改rigidbody velocity之类的应该放在对应的State类里
        /// </summary>
        /// <param name="inputHandler"></param>
        public override void BindInput(AdvPlayerInput inputHandler)
        {
            base.BindInput(inputHandler);
            playerInput.Lock.started += context => IsLockPressed = true;
            playerInput.Lock.canceled += context => IsLockPressed = false;

            playerInput.Hook.performed += context => IsHookPressed = true;
            playerInput.Hook.started += context =>
            {
                IsHookPressed = false;
                IsHookPressing = true;
            };
            playerInput.Hook.canceled += context =>
            {
                IsHookPressed = false;
                IsHookPressing = false;
            };

            playerInput.Interact.performed += context => IsInteractPressed = true;
            //playerInput.Interact.started += context => IsInteractPressed = true;
            playerInput.Interact.canceled += context => IsInteractPressed = false;

            playerInput.ChangeWeaponRight.performed += context => IsChangeWeaponRightPressed = true;
            playerInput.ChangeWeaponRight.canceled += context => IsChangeWeaponRightPressed = false;

            playerInput.ChangeWeaponLeft.performed += context => IsChangeWeaponLeftPressed = true;
            playerInput.ChangeWeaponLeft.canceled += context => IsChangeWeaponLeftPressed = false;

            playerInput.Attack.performed += context => IsAttackPressed = true;
            //playerInput.Attack.started += context => IsAttackPressed = true;
            playerInput.Attack.canceled += context => IsAttackPressed = false;
        }

        #endregion

        #region State相关方法
        //---------------------------------------<State相关方法>--------------------------------------------------



        public override void TurnFace()
        {
            Vector3 en_flip = base.Transform.localScale;
            en_flip.x *= -1f;

            if (inputAxis.x > 0 && base.Transform.localScale.x < 0) base.Transform.localScale = en_flip;
            else if (inputAxis.x < 0 && base.Transform.localScale.x > 0) base.Transform.localScale = en_flip;
            else { }
        }

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

        public void Collect()
        {

        }

        public event Func<DamageData> OnAttacked;
        public void GetDamage()
        {
            var data = OnAttacked?.Invoke();
            if (data == null) return;
            data.defValue = defValue;
            float finalDamageValue = DamageSystem.CalculateDamage(data);
            HP -= (int)finalDamageValue;
        }

        public void changeWeapon()
        {
            //手动设置IsChangeWeaponPressed防止连按
            if (IsChangeWeaponLeftPressed) { CurrentWeaponIndex -= 1; IsChangeWeaponLeftPressed = false; }
            else if (IsChangeWeaponRightPressed) { CurrentWeaponIndex += 1; IsChangeWeaponRightPressed = false; }
            else
            {
                Debug.Log("切换武器失败");
                return;
            }
            currentWeapon.GameObject.SetActive(false);//切换前的武器应该隐藏
            currentWeapon = availableWeapons[CurrentWeaponIndex];
            currentWeapon.GameObject.SetActive(true);
            Debug.Log($"切换武器为{currentWeapon.GameObject.name}");
        }

        public void Shoot()
        {
            WeaponManager.SpawnBullet(currentWeapon, this);
        }

        //Die可以理解为和一切初始化相反的方法
        //public void Die()
        //{

        //    updater.RemoveUpdateFunction(OnUpdate);
        //    updater.RemoveFixedUpdateFunction(OnFixedUpdate);

        //    //UnityEngine.Object.DestroyImmediate()
        //    UnityEngine.Object.Destroy(this.GameObject);
        //    PlayerManager.m_Role = null;//其余等C#自带GC回收？
        //}

        //---------------------------------------<生命周期>--------------------------------------------------

        #endregion

        #region 生命周期 

        protected override void OnUpdate(float deltaTime)
        {
            generalFsm.Update(deltaTime);
            subFsm.Update(deltaTime);


            if (isInInteractArea) { nearestInteractableGobj = ADVSceneGobjManager.GetNearest(base.Transform.position, GobjsInInteractArea); }//实时计算距离最近对象
            if (IsChangeWeaponLeftPressed || IsChangeWeaponRightPressed) { changeWeapon(); }
        }

        //物理相关的刷新
        protected override void OnFixedUpdate(float fixedDeltaTime)
        {
            generalFsm.FixedUpdate(fixedDeltaTime);

            subFsm.FixedUpdate(fixedDeltaTime);

            if (canApplyGravity) ApplyGravity(fixedDeltaTime);
        }

        #endregion



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


}





