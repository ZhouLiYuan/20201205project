using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//受击能力升级后，收集用的collider半径会变大
public class Collector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Item")) 
        {
            Destroy(collision.gameObject);//被收集后销毁
        }
    }
}
