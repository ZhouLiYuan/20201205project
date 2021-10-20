using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseState : StateMachineBehaviour
{
    ////提供角色追踪方位
    //protected GameObject playerGobj => Enemy.roleGobj;
    //protected PlayerRole player => Enemy.role;
    //protected Transform playerTransform => Enemy.pl_Transform;


    ////敌人自身
    //public string en_instanceName => Enemy.en_name;
    ////自身字段第一层级
    //protected GameObject en_TopNodeGobj =>Enemy.en_gameObject;
    //protected Transform en_TopNodeTransform => Enemy.en_transform;
    ////自身字段第二层级
    //protected GameObject en_AnimatorGobj => Enemy.en_animatorGobj;

    //protected Rigidbody2D en_rb2d => Enemy.en_rb2d;


    ////配置表属性
    //protected float en_speed => Enemy.speed;
    //protected float attackRange => Enemy.attackRange;
    //protected float chaseRange => Enemy.chaseRange;


    ////敌人与角色关系
    //protected float distanceToPlayer => Enemy.distanceToPlayer;
    //逻辑层

    //key
    protected Collider2D en_HitCollider;
    //Instance
    protected Enemy Enemy { get; private set; }



    //提供角色追踪方位
    protected GameObject playerGobj;
    protected PlayerRole player;
    protected Transform playerTransform;


    //敌人自身
    //自身字段第一层级
    protected GameObject en_TopNodeGobj;
    protected Transform en_TopNodeTransform;
    protected Rigidbody2D en_rb2d;


    //自身字段第二层级
    protected GameObject en_AnimatorGobj;
    //地面检测
    public GroundDetect en_GroundDetect { get; private set; }


    //配置表属性
    protected float en_speed;
    protected float attackRange;
    protected float chaseRange;


    //敌人与角色关系
    protected float distanceToPlayer;



    protected void Init()
    {
        Enemy = EnemyManager.GetInstanceByCollider(en_HitCollider);

        playerGobj = Enemy.roleGobj;
        player = Enemy.role;
        playerTransform = Enemy.pl_Transform;



        en_TopNodeGobj = Enemy.GameObject;
        en_TopNodeTransform = Enemy.Transform;
        en_rb2d = Enemy.rg2d;

        en_GroundDetect = Enemy.GroundDetect;



        //配置表属性
        en_speed = Enemy.speed;
        attackRange = Enemy.attackRange;
        chaseRange = Enemy.chaseRange;

    }


    /// <summary>
    /// 让敌人自动面朝玩家
    /// 逻辑：
    ///理解为判断 相对 角色(Enemy prefab)初始朝向（朝右），是否以反转过
    ///true已反转（向左） false未反转（朝右）
    ///敌人朝向 和 玩家的朝向 无关 和 玩家方位有关
    /// 注意事项：
    /// 如果在animation里K了脚本中修改的参数帧，那么这个脚本的中的数据修改都将无效
    /// 目前解决方案，追加多个topNode用作反转，通过字Gobj修改父Gobj参数
    /// 反转的是父级的transform.localScale
    /// 教程地址https://www.youtube.com/watch?v=AD4JIXQDw0s 12分42秒
    /// </summary>
    protected void LookAtPlayer()
    {

        //if语句判断 要不要把反转后的向量 赋值给en_TopNodeTransform
        Vector3 en_flip = en_TopNodeTransform.localScale;
        en_flip.x *= -1f;

        //当Enemy在Player右边，并且Enemy还没有反转过（当前向右）
        if (en_TopNodeTransform.position.x > playerGobj.transform.position.x && en_TopNodeTransform.localScale.x >0 ) 
        {en_TopNodeTransform.localScale = en_flip;}
        //当Enemy在Player左边，并且Enemy反转过（当前向左）
        else if (en_TopNodeTransform.position.x < playerGobj.transform.position.x && en_TopNodeTransform.localScale.x <0)
        {en_TopNodeTransform.localScale = en_flip;}
        //位置重合的情况，或其他情况
        else { }
    }

    protected void ChasePlayer()
    {
        //只追踪player的横坐标，无需追踪Y轴，故Y方向目标点设为自身
        Vector2 targetPosition = new Vector2(playerTransform.position.x, en_rb2d.position.y);
        //en_newPosition只是enemy需要移动到的目标矢量Pos，而不是enemy刚体本身
        Vector2 movementVector = Vector2.MoveTowards(en_rb2d.position, targetPosition, en_speed * Time.fixedDeltaTime);
        //实际修改敌人刚体矢量

        //这个API好像会让en_rb2d的重力无效化
        // * en_speed
        en_rb2d.MovePosition(movementVector);

    }

    protected void Attack() 
    {
    }
}




