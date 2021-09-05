using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapon;

//教程https://www.youtube.com/watch?v=AD4JIXQDw0s 15分40秒
public class Sword : BaseWeapon/*LightMelee*/
{
    public Sword() 
    {
        atkValue = 5f;
        atkType = AtkType.normal;
    }
}
