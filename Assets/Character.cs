using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] public GameObject graphic;
    //public Vector2 velocity;
    //下面其实是x的max速度
    public float maxSpeed;
    Vector3 dir = new Vector3(0, -1);
    //size不同的角色检测里面距离不同  
    [SerializeField] private float distance = 0.2f;
    [SerializeField] private float jumpSpeed = 5f;

    [SerializeField] private Transform groundCheckPosition;
    [SerializeField] Rigidbody2D rigidbody2d;
    //好像这里的rigidbody2d和所挂载Gobj的rg2d组件没什么关系，序列化后并没有出现在inspector里
    [SerializeField] private Hook m_hook;

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
        //为什么不用Vector3要用var？
        var worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //挂载Gobj坐标 到 鼠标坐标(实现实时变化的距离)
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