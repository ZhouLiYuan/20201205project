using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 只用碰撞事件通知不用其生命周期函数
/// </summary>
public class GroundDetect : MonoBehaviour
{
    public bool IsGrounded { get; private set; }
    [SerializeField] private Transform groundCheckPosition;
    [SerializeField] private float distance = 0.2f;


    ////不继承MONO的赋值版本
    //public GroundDetect(Transform groundDetectGobjTransform) 
    //{
    //    groundCheckPosition = groundDetectGobjTransform;
    //}
    //  var characterTransform = GameObject.Find("character").transform;

    private void OnCollisionStay2D(Collision2D collision)
    {
        // 碰到符合条件的Collider，认为落地
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
        Debug.DrawLine(groundCheckPosition.position, groundCheckPosition.position + transform.up * -1f * distance, Color.red, 1f);
    }
}