using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] public GameObject m_character;
    //public Vector2 velocity;
    /// <summary>
    /// x轴的max速度
    /// </summary>
    public float maxSpeed;
    Vector3 dir = new Vector3(0, -1);

    /// <summary>
    /// 剩余可跳跃次数
    /// </summary>
    private int jumpCount;
    [SerializeField] private float jumpSpeed;
    [SerializeField] Rigidbody2D rigidbody2d;

    //能力延伸
    [SerializeField] private Hook m_hook;

    //UI部分
    [SerializeField] private GameObject weaponSelectCanvas;
    [SerializeField] private GameObject weapon;

    //地面检测
    private GroundDetect _groundDetect;
    private bool IsGrounded { get { return _groundDetect.IsGrounded; } }

    protected Vector2 targetVelocity;
    protected Vector2 move;


    private void Start()
    {
        move = Vector2.zero;
        _groundDetect = GetComponentInChildren<GroundDetect>();
    }


    private void Update()
    {

        //在自己类体里调用就不用声明实例
        HandleInput();
        GraphicFlip();

    }



    private void HandleInput()
    {
        //移动
        move.x = Input.GetAxis("Horizontal");
        HorizontalMovement();
        //跳跃
        if (Input.GetKeyDown(KeyCode.Space)) { Jump(); }
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

    /// <summary>
    ///角色反转功能
    ///使用前提，动画中没有k scale或者rotation的帧
    ///当Gobj scale和速度方向相反的时候反转
    /// </summary>
    private void GraphicFlip()
    {
        //先判断非空
        if (m_character)
        {
            if (move.x > 0.01f && m_character.transform.localScale.x == -1)
            {
                m_character.transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
            }
            else if (move.x < -0.01f && m_character.transform.localScale.x == 1)
            {
                    m_character.transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
            }
        }
    }

    /// <summary>
    /// 一次跳跃，2段跳，浮空跳
    /// </summary>
    private void Jump()

    {
        if (IsGrounded)
        {
            jumpCount = 2;
            rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, jumpSpeed);
            jumpCount--;
            Debug.Log($"剩余跳跃次数{jumpCount}");
        }
        //
        else if (Input.GetKeyDown(KeyCode.Space) && !IsGrounded && jumpCount > 0)
        {
            rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, jumpSpeed);
            jumpCount = 0;
        }
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


    //切换武器
    public void ChangeWeapon(string weaponName)
    {
        weapon = Resources.Load<GameObject>(weaponName);
        Debug.Log("the weapon has been changed to" + weaponName);
        weaponSelectCanvas.SetActive(false);
    }

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


