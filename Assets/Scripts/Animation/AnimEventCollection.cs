using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimEventCollection : MonoBehaviour
{
    public delegate void AnimtionEvent();//无参无返回值委托（可以用UnityAction API代替）

    #region Unity事件API
    // 事件拥有者
    public event UnityAction eventSource_OnJump, eventSource_OnLand, eventSource_OnHardLand;
    //事件
    public UnityEvent OnJump, OnLand, OnHardLand;

    public void Init()
    {
        eventSource_OnLand += OnLand.Invoke;
        eventSource_OnJump += OnJump.Invoke;
        eventSource_OnHardLand += OnHardLand.Invoke;
    }
    #endregion

    public event AnimtionEvent EventOne;
    public  void InvokeEventOne () { EventOne?.Invoke(); }

    public event AnimtionEvent AttackEvent;
    public void Attack() { AttackEvent?.Invoke(); }

    //依次类推
}
