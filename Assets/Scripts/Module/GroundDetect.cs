using UnityEngine;



public class GroundDetect 
{
    public bool IsGrounded { get; private set; }
    private Transform groundCheckPosition;
    ///需要用插件序列化的变量！！！
    [SerializeField] private float distance = 0.2f;

    public GroundDetect(Transform groundDetectGobjTransform) 
    {
        groundCheckPosition = groundDetectGobjTransform;
    }

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
        var characterTransform = GameObject.Find("character").transform;
        Debug.DrawLine(groundCheckPosition.position, groundCheckPosition.position + characterTransform.up * -1f * distance, Color.red, 1f);
    }
}