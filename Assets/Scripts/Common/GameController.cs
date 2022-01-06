using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class GameController
{
    public virtual void StartGame(int level) { }
    public virtual void StartGame(int P1_characterID, int P2_characterID) { }
    public abstract void ExitGame();
}
