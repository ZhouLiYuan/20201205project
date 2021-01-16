using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] public GameObject graphic;
    public Vector2 velocity;
    //下面其实是x的max速度
    public float maxSpeed;

    [SerializeField] private Transform groundCheckPosition;
    //size不同的角色检测里面距离不同  
    [SerializeField] private float distance = 0.2f;
    [SerializeField] private float jumpSpeed = 10f;
    [SerializeField] Rigidbody2D rigidbody2d;
    //好像这里的rigidbody2d和所挂载Gobj的rg2d组件没什么关系


    private bool isGrounded;

    protected Vector2 targetVelocity;
    protected Vector2 move = Vector2.zero;


    private void Update()
    {
        //在自己类体里调用就不用声明实例
        HandleInput();
        HorizontalMovement();

        CheckIsGrounded();
        OnDrawGizmos();

    }



    private void HandleInput()
    {
        move.x = Input.GetAxis("Horizontal");
        if (isGrounded && Input.GetKeyDown(KeyCode.Space)) { Jump(); }
    }

    private void Jump()

    {
        rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, jumpSpeed);
    }

    public void HorizontalMovement()
    {
        rigidbody2d.velocity = new Vector2(move.x * maxSpeed, rigidbody2d.velocity.y);
    }

    public void CheckIsGrounded()
    {
        //只在这个方法内有用的变量就声明为本地变量，节省可用的公开变量名
        RaycastHit hit;
        Vector3 dir = new Vector3(0, -1);

        isGrounded = (Physics2D.Raycast(groundCheckPosition.position, dir, distance).collider.tag == "Ground");

    }

    private void OnDrawGizmos()
    {
        Debug.DrawLine(groundCheckPosition.position, groundCheckPosition.position + transform.up * -1f * distance, Color.red,1f);
    }


}