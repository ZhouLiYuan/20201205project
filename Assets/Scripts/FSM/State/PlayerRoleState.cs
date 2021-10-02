using UnityEngine;

/// <summary>
/// 角色状态
/// </summary>
public class PlayerRoleState:State
{
   public PlayerRole Role { get; private set; }
    //lambda表达式写get访问器
    protected GameObject role_Gobj => Role.GameObject;
    protected GroundDetect GroundDetect => Role.GroundDetect;

    protected Vector2 InputAxis => Role.inputAxis;

    public float jumpSpeed = 8f;
    public float moveSpeed = 5f;

    //消灭 空引用
    public void SetPlayerRole(PlayerRole role)
    {
        Role = role;
    }
    //把Role中的Velocity包装到当前状态类中，更方便访问修改(上面的其他属性也在做同样的事情（只是他们只有get方法）)
    protected Vector2 Velocity
    {
        get { return Role.Velocity; }
        set { Role.Velocity = value; }
    }
}
