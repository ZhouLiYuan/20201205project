using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//教程地址https://www.youtube.com/watch?v=AD4JIXQDw0s 12分42秒
/// <summary>
/// 让敌人自动面朝玩家
/// </summary>
public class EnemyFacing : MonoBehaviour
{
    //理解为判断 相对角色(Enemy prefab)初始朝向（朝右），是否以反转过
    bool isFlipped = true;
    Transform player;



    //敌人的朝向 和 玩家的朝向 没有关联 和 玩家的方位有关联
    public void LookAtPlayer()
    {
        
        //获取玩家信息
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
        //先计算好反转后的向量，再用if语句判断要不要把这个向量赋值给Enemy
        Vector3 en_flip = transform.localScale;
        en_flip.z *= -1f;


        //当Enemy在Player右边，并且Enemy还没有反转过（当前向右）
        if (transform.position.x < player.position.x && !isFlipped)
        {
            transform.localScale = en_flip;
            //这一步是为了确保Enemy不是通过rotate而是通过scale旋转的？
            transform.Rotate(0f, 180f, 0f);
            //设为与判断条件相反的值
            isFlipped = true;
        }

        //当Enemy在Player左边，并且Enemy反转过（当前向左）
        else if (transform.position.x > player.position.x && isFlipped)
        {
            transform.localScale = en_flip;
            //这一步是为了确保Enemy不是通过rotate而是通过scale旋转的？
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
        }

        //位置重合的情况，或者其他情况，什么都不做
        else { }

    }

}
