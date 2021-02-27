using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] public GameObject graphic;
    //public Vector2 velocity;
    /// <summary>
    /// x轴的max速度
    /// </summary>
    public float maxSpeed;
    Vector3 dir = new Vector3(0, -1);

    //size不同的角色检测离地距离不同（也许可以做成模块,下面是代码参考，和GroundDetect配套）
    //[SerializeField] private Rigidbody2D _rigidbody;

    //private GroundDetect _groundDetect;
    //private bool IsGrounded { get { return _groundDetect.IsGrounded; } }

    //void Start()
    //{
    //    _rigidbody = GetComponent<Rigidbody2D>();
    //    _groundDetect = GetComponentInChildren<GroundDetect>();
   
    //}

    [SerializeField] private float distance = 0.2f;
    [SerializeField] private float jumpSpeed = 5f;

    [SerializeField] private Transform groundCheckPosition;
    //好像这里的rigidbody2d和所挂载Gobj的rg2d组件没什么关系
    //序列化后并没有出现在inspector里
    [SerializeField] Rigidbody2D rigidbody2d;
    
    //能力延伸
    [SerializeField] private Hook m_hook;

    //UI部分
    [SerializeField] private GameObject weaponSelectCanvas;
    [SerializeField] private GameObject weapon;

    private bool isGrounded;

    protected Vector2 targetVelocity;
    protected Vector2 move = Vector2.zero;



    private void Update()
    {

        //在自己类体里调用就不用声明实例
        CheckIsGrounded();

        HandleInput();

    }



    private void HandleInput()
    {
        //移动
        move.x = Input.GetAxis("Horizontal");
        HorizontalMovement();
        //跳跃
        if (isGrounded && Input.GetKeyDown(KeyCode.Space)) { Jump(); }
        //钩锁
        if (Input.GetKey(KeyCode.G))
        {
            Hook();
        }
    }


    public void HorizontalMovement()
    {
        Debug.Log($"水平输入 = {move.x}");
        rigidbody2d.velocity = new Vector2(move.x * maxSpeed, rigidbody2d.velocity.y);
    }

    private void Jump()

    {
        rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, jumpSpeed);
    }

    private void Hook()
    {
        
        var worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //所挂载Gobj坐标 到 鼠标坐标(实现实时变化的距离)
        var direction = worldPosition - transform.position;
        Debug.DrawLine(transform.position, worldPosition, Color.green);

        //防止射线撞到挂载Gobj本身，需要设置忽略层,参数列表再追加多一个LayerMask
        var result = Physics2D.Raycast(transform.position, direction, direction.magnitude, LayerMask.GetMask("Platform"));

        if (result.collider && result.collider.tag == "Platform")
        {
            Debug.Log("发射钩锁");
            m_hook.Shoot(result.point);
            //开按键才会飞过去
        }

    }

    private void CheckIsGrounded()
    {
        var result = Physics2D.Raycast(groundCheckPosition.position, dir, distance);
        //需要先判断result非空
        if (result.collider)
        {
            isGrounded = result.collider.tag == "Platform";
        }
        //补足result为空的情况，因为result为空的时候不会执行上面的if语句， isGrounded会一直保留最开始时接地的判断true,会导致玩家可以无限跳
        else { isGrounded = false; }

        //Debug.Log($"{result.collider}{isGrounded}");
        Debug.Log($"地面碰撞情况 = {isGrounded}");
    }

    private void OnDrawGizmos()
    {
        Debug.DrawLine(groundCheckPosition.position, groundCheckPosition.position + transform.up * -1f * distance, Color.red, 1f);
    }


    //切换武器
    public void ChangeWeapon(string weaponName)
    {
        weapon = Resources.Load<GameObject>(weaponName);
        Debug.Log("the weapon has been changed to" + weaponName);
        weaponSelectCanvas.SetActive(false);
    }

}


//代码素材库
/*
[SerializeField] private GameObject graphic;
[SerializeField] private Animator animator;
[SerializeField] private bool jumping;
[SerializeField] private AudioSource audio;
[SerializeField] private AudioClip[] stepSounds;
[SerializeField] private AudioClip[] jumpSounds;
*/

//反转功能
//if (graphic)
//{
//    if (move.x > 0.01f)
//    {
//        if (graphic.transform.localScale.x == -1) //这里是不是写反了
//        {
//            graphic.transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
//            //为什么变量后面可以跟成员名
//            //前面加上GameObject变量名反而错？
//        }
//    }
//    else if (move.x < -0.01f)
//    {
//        if (graphic.transform.localScale.x == 1)
//        {
//            graphic.transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
//        }
//    }

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


//二段跳
//  void Jump()
//{
//    if (isGround)
//    {
//        //二段跳
//        jumpCount = 2;
//        isJump = false;
//    }
//    if (jumpPressed && isGround)
//    {
//        isJump = true;
//        Debug.Log(m_rb.velocity);
//        m_rb.velocity = new Vector2(m_rb.velocity.x, jumpTakeOffSpeed);
//        Debug.Log(m_rb.velocity);
//        jumpCount--;
//        jumpPressed = false;//这算是手动重置按键吗
//    }
//    else if (jumpPressed && isJump && jumpCount > 0) //isJump限制这是第二段跳
//    {
//        m_rb.velocity = new Vector2(m_rb.velocity.x, jumpTakeOffSpeed);
//        jumpCount--;
//        jumpPressed = false;
