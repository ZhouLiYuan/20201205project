using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 只用 碰撞事件通知 不用 生命周期函数
/// </summary>
public class GroundDetect : MonoBehaviour
{
    public bool IsGrounded { get; private set; }
    //[SerializeField] private float distance = 0.2f;
    //[SerializeField] private GameObject groundCheckGobj;

    //public GroundDetect(GameObject Gobj)
    //{
    //    groundCheckGobj = Gobj;
    //}
   

    private void OnCollisionStay2D(Collision2D collision)
    {
       if (collision.gameObject.tag == "Platform")
        {
            IsGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            IsGrounded = false;
        }
    }

    private void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, transform.position + transform.up * -0.2f/* * distance*/, Color.blue, 1f);
    }
}