using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookedChecker

{
    public GameObject player;
    //这是个碰撞触发事件？other好像没查到功能定义
    private void OnTriggerEnter(Collider2D other)
    {

        if (other.tag == "Hookable")
        {
            //从别的脚本里获取参数(脚本挂再GObj上后也变成了一个component)
            player.GetComponent<GrapplingHook>().hooked = true;
            player.GetComponent<GrapplingHook>().hookedObj = other.gameObject;
            
        }
    }

}
