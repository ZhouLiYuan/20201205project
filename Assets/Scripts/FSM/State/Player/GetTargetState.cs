using Role.Enemies;
using UnityEngine;


namespace Role
{
    namespace SelectableRole
    {
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
                direction = role_Gobj.transform.position - TargetEnemy.Transform.position;//指向Player的方向

                HookTransform.position = TargetEnemy.Transform.position;//hook黏在敌人身上
                                                                        //HookTransform.position = new Vector3(1.2f,0.5f,0f) + Role.Transform.position;//把hook想象成一个骨骼位置
                GenerateHookEffect(HookPos);

                //敌人离角色的最小距离
                var minColliderDistance = (Role.HitCollider.bounds.size + TargetEnemy.HitCollider.bounds.size) / 2;
                if (direction.sqrMagnitude > minColliderDistance.sqrMagnitude + 1)
                {
                    //怎样做到速度合理渐变
                    var k = Vector2.Distance(Role.Transform.position, TargetEnemy.Transform.position) - 1;//-1减去玩家和敌人自身体型的半径和
                    if (k < 0) k = 0;
                    if (k > 1) k = 1;
                    Debug.Log($"斜率值{k} ");
                    grapSpeed = grapSpeed * k;
                    //grapSpeed = Mathf.Lerp(grapSpeed, grapSpeed * k, k);
                    //grapSpeed = Mathf.Lerp(grapSpeed, grapSpeed* 0.5f, deltaTime);
                    TargetEnemy.rg2d.velocity = direction.normalized * grapSpeed;
                    Debug.Log($"抓取状态1方向{direction}最小间距 {minColliderDistance}");
                }
                else if (direction.sqrMagnitude <= minColliderDistance.sqrMagnitude + 1)//这样写和只写else有什么区别（为什么写else就没反应？）
                {
                    Debug.Log("抓取状态2");
                    //TargetOwnerTransform最好别用，因为未必拿到Hookable信标
                    TargetEnemy.Transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    TargetEnemy.Transform.position = Role.Transform.position - direction.normalized * 1.3f;//+个offset(这个数值直接决定了能不能精确维持else if的状态)
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
    }
}