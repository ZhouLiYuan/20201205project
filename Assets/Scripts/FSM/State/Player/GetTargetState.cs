using UnityEngine;

public class GetTargetState : HookBaseState
{
    private float grapSpeed;

    //hookable逻辑Gobj与其父级Owner的信息
    private string TargetOwnerName => Role.lockTarget.transform.parent.gameObject.name;
    private Enemy TargetEnemy;

    //禁用重力 和 输入对左右移动的控制
    public override void OnEnter()
    {
        base.OnEnter();
        TargetEnemy = EnemyManager.GetInstanceByName(TargetOwnerName);
        grapSpeed = Role.grapSpeed;
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
        TargetEnemy.animator.SetTrigger("Grabbed");
        if (!Role.IsHookPressing) ChangeState<PreSubActionState>();
        direction = role_Gobj.transform.position - TargetPos;

        HookTransform.position = TargetEnemy.Transform.position;//hook黏在敌人身上
        //HookTransform.position = new Vector3(1.2f,0.5f,0f) + Role.Transform.position;//把hook想象成一个骨骼位置
        GenerateHookEffect(HookPos);
        if (direction.sqrMagnitude > 1)//1是敌人离角色的最小距离
        {
            //怎样做到速度合理渐变
            //var k = Vector2.Distance(Role.Transform.position, TargetPos);
            //grapSpeed = Mathf.Lerp(grapSpeed, grapSpeed * k, k);
            //grapSpeed = Mathf.Lerp(grapSpeed, grapSpeed* 0.5f, deltaTime);
            TargetEnemy.Transform.GetComponent<Rigidbody2D>().velocity = direction.normalized * grapSpeed;
        }
        else//抓取物已到角色附近
        {
            TargetOwnerTransform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            TargetOwnerTransform.position = Role.Transform.position - direction.normalized;//-个offset
            //TargetOwnerTransform.SetParent(HookTransform, true);
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        //TargetEnemy.Transform.SetParent(EnemyManager.s_listTransform, true);
        Role.lockTarget = null;
        TargetEnemy = null;
    }
}
