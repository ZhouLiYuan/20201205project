using UnityEngine;

public class GroundDetect : MonoBehaviour
{
    public bool IsGrounded { get; private set; }
    [SerializeField] private Transform groundCheckPosition;
    [SerializeField] private float distance = 0.2f;

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