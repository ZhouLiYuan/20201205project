
/// <summary>
/// 状态方面，只有ChangeState方法（封装自FSM）
/// </summary>
public class State
{
    public string Name { get; private set; }
    public FSM FSM { get; private set; }


    /// <summary>
    /// 获取(继承自State的具体状态类)类型名
    /// </summary>
    public State()
    {
        Name = this.GetType().Name;
    }

    //可选择调用与否
    internal void InitFSM(FSM fsm)
    {
        FSM = fsm;
    }

    protected TState ChangeState<TState>() where TState : State
    {
        return FSM.ChangeState<TState>();
    }

    public virtual void OnEnter() { }
    public virtual void OnUpdate(float deltaTime) { }
    public virtual void OnFixedUpdate(float fixedDeltaTime) { }
    public virtual void OnExit() { }

}
