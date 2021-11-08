using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEventCollection : MonoBehaviour
{
    public delegate void AnimtionEvent();//无参无返回值委托


    public  event AnimtionEvent EventOne;
    public  void InvokeEventOne () { EventOne?.Invoke(); }

    public event AnimtionEvent AttackEvent;
    public void Attack() { AttackEvent?.Invoke(); }

    //依次类推
}
