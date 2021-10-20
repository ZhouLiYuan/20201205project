public class RoleConfig:Config
{
	public int HP; // 生命值
}

public class NPCConfig : RoleConfig
{
}

public class EnemyConfig:RoleConfig
{
	public float Speed; // 移动速度
	public float AttackRange; // 攻击范围
	public float ChaseRange; // 追踪范围
}

public class PlayerRoleConfig : RoleConfig
{
    public int ATK; // 基础攻击力
    public int DEF; // 基础防御力
    public float JumpSpeed; // 基础跳跃值
    public float MoveSpeed; // 基础移动值
}