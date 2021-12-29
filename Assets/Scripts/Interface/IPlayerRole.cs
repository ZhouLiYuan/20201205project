using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//接口主要是定义功能规范
public interface IPlayerRole
{
    //DamageReceiver DamageReceiver { get; set; }

    #region State相关方法
    //---------------------------------------<State相关方法>--------------------------------------------------
    void ApplyGravity(float fixedDeltaTime);
    void OnEnterInteractArea(GameObject target);
    void Interact();
    void ExitInteract();
    void ExitInteractArea(GameObject target);
    void Collect();
    void GetDamage();
    void changeWeapon();
    void Shoot();
    void Die();
    //---------------------------------------<生命周期>--------------------------------------------------

    #endregion
}