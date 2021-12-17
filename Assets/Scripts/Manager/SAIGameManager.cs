
//抽象多一层GameController的管理层
public static class SAIGameManager
{
    private static GameController m_gameController;

    /// <summary>
    /// 需要传入关卡<see cref="level"/>index
    /// </summary>
    /// <typeparam name="TGameController"></typeparam>
    /// <param name="level"></param>
    public static void StartAdvGame<TGameController>(int level) where TGameController : GameController, new() //用于ADV
    {
        m_gameController = new TGameController();
        m_gameController.StartGame(level);
    }

    public static void StartBtlGame<TGameController>(int P1_characterID = 0, int P2_characterID = 1) where TGameController : GameController, new()//用于BTL
    {
        m_gameController = new TGameController();
        m_gameController.StartGame(P1_characterID, P2_characterID);
    }

    public static void ExitGame() 
    {
        m_gameController.ExitGame();
    }

}
