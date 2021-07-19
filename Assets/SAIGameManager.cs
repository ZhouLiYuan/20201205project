using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//抽象多一层GameController的管理层
public static class SAIGameManager
{
    private static GameController m_gameController;

    public static void StartGame<TGameController>(int level) where TGameController : GameController, new() 
    {
        m_gameController = new TGameController();
        m_gameController.StartGame(level);
    }

    public static void ExitGame() 
    {
        m_gameController.ExitGame();
    }

}
