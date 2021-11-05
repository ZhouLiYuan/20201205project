using Role;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordEnemy:Enemy
{
    public override void Attack()
    {
    }

    public override void ChasePlayer()
    {
        //只追踪player的横坐标，无需追踪Y轴，故Y方向目标点设为自身
        Vector2 targetPosition = new Vector2(pl_Transform.position.x, rg2d.position.y);
        //en_newPosition只是enemy需要移动到的目标矢量Pos，而不是enemy刚体本身
        Vector2 movementVector = Vector2.MoveTowards(rg2d.position, targetPosition, speed * Time.fixedDeltaTime);
        //实际修改敌人刚体矢量

        //这个API好像会让en_rb2d的重力无效化
        // * en_speed
        rg2d.MovePosition(movementVector);
    }
}
