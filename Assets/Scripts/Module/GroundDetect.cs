using UnityEngine;

public class GroundDetect : MonoBehaviour
{
    public bool IsGrounded { get; private set; }


    private void OnCollisionStay2D(Collision2D collision)
    {
        // 碰到符合条件的Collider，认为落地
        if (true)
        {
            IsGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (true)
        {
            IsGrounded = false;
        }
    }
}