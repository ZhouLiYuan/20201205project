using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedCheck
{
    public GameObject character;
    public bool grounded;
    

    public void CheckIsGrounded()
    {
        //只在这个方法内有用的变量就声明为本地变量，节省可用的公开变量名
        RaycastHit hit;
        float distance = 0.5f;
        Vector3 dir = new Vector3(0, -1);


        if (Physics.Raycast(character.transform.position, dir, out hit, distance))
        {
            grounded = true;
        }

        else
        {
            grounded = false;
        }

    }
}
