using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.one);
    }
}
