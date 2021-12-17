namespace Role.SelectableRole
{
        public class SwordAttackState : PlayerRoleState
        {
            public override void OnEnter()
            {
                base.OnEnter();
                Role.canMoveHorizontal = false;
                Role.currentWeapon.collider2D.enabled = true;
            }

            public override void OnUpdate(float deltaTime)
            {
                AnimDeltaTime += deltaTime;
                //Velocity = new Vector2(InputAxis.x * MoveSpeed, Velocity.y);//横向输入 x轴方向
                //Debug.Log($"{animClipName}动画已播放{AnimDeltaTime}秒 ");
                if (AnimDeltaTime >= currentAnim.length) ChangeState<PreSubActionState>(); //动画播放完毕切回准备攻击状态（AnimDeltaTime比动画长度长就确保播完了）
            }

            public override void OnExit()
            {
                Role.canMoveHorizontal = true;
                Role.currentWeapon.collider2D.enabled = false;
            }
        }
}