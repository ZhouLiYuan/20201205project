using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump: Movement
{
    public float maxSpeed = 7;
    public float jumpTakeOffSpeed;
    //跳跃次数
    public float jumpCount;

    private Rigidbody2D m_rb;
 
    public bool isJump;
    //是否按下了跳跃按钮
    public bool jumpPressed;

    //好像不用new实例也可以？
    public CharacterExecute jumpCE;


    public void Jumpping()
    {
        if (Input.GetButtonDown("Jump") && jumpCE.m_groundCheck.grounded)
        {
            velocity.y = jumpTakeOffSpeed;
        }
        else if (Input.GetButtonUp("Jump"))
        {
            if (velocity.y > 0)
            {
                velocity.y = velocity.y * 0.5f;
            }
        }

        if (jumpCE.m_groundCheck.grounded)
        {
            //二段跳
            jumpCount = 2;
            isJump = false;
        }
        if (jumpPressed && jumpCE.m_groundCheck.grounded)
        {
            isJump = true;
            Debug.Log(m_rb.velocity);
            m_rb.velocity = new Vector2(m_rb.velocity.x, jumpTakeOffSpeed);
            Debug.Log(m_rb.velocity);
            jumpCount--;
            jumpPressed = false;//这算是手动重置按键吗
        }
        else if (jumpPressed && isJump && jumpCount > 0) //isJump限制这是第二段跳
        {
            m_rb.velocity = new Vector2(m_rb.velocity.x, jumpTakeOffSpeed);
            jumpCount--;
            jumpPressed = false;

        }

    }
}
