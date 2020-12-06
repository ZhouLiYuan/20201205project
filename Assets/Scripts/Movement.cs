using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Movement:MonoBehaviour
{
    [SerializeField] public GameObject graphic;

    protected Vector2 targetVelocity;
    protected Vector2 move = Vector2.zero;
    protected Rigidbody2D rb2d;

    public Vector2 velocity;

    public float maxSpeed;
    

    public void HorizontalMovement()
    {
        

        move.x = Input.GetAxis("Horizontal");

     
        targetVelocity = move * maxSpeed;
    }

    //一些可参考代码

    //public float minGroundNormalY = .65f;
    //public float gravityModifier = 1f;

    //protected Vector2 targetVelocity;
    //protected bool grounded;
    //protected Vector2 groundNormal;
    //protected Rigidbody2D rb2d;
    //protected Vector2 velocity;
    //protected ContactFilter2D contactFilter;
    //protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    //protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);


    //protected const float minMoveDistance = 0.001f;
    //protected const float shellRadius = 0.01f;

    //public void Move(Vector2 move, bool yMovement)
    //{
    //    float distance = move.magnitude;

    //    if (distance > minMoveDistance)
    //    {
    //        int count = rb2d.Cast(move, contactFilter, hitBuffer, distance + shellRadius);
    //        hitBufferList.Clear();
    //        for (int i = 0; i < count; i++)
    //        {
    //            hitBufferList.Add(hitBuffer[i]);
    //        }

    //        for (int i = 0; i < hitBufferList.Count; i++)
    //        {

    //            //这里用法线命名是什么意思？
    //            Vector2 currentNormal = hitBufferList[i].normal;
    //            if (currentNormal.y > minGroundNormalY)
    //            {
    //                grounded = true;
    //                if (yMovement)
    //                {
    //                    groundNormal = currentNormal;
    //                    currentNormal.x = 0;
    //                }
    //            }

    //            float projection = Vector2.Dot(velocity, currentNormal);
    //            if (projection < 0)
    //            {
    //                velocity = velocity - projection * currentNormal;
    //            }

    //            float modifiedDistance = hitBufferList[i].distance - shellRadius;
    //            distance = modifiedDistance < distance ? modifiedDistance : distance;
    //        }


    //    }

    //    rb2d.position = rb2d.position + move.normalized * distance;
    //}


}
