using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    public Transform m_cam;

    //作用 固定住UI一直面向屏幕
    void LateUpdate()
    {
        //这里有点没看懂为什么要 +
        transform.LookAt(transform.position + m_cam.forward);
    }
}
