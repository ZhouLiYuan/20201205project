using UnityEngine;
using UnityEngine.UI;

//目前设计挂在NPC或者交互物的Top_Node上
public class PlayerInteractDetect : MonoBehaviour
{
    //角色正对着才会检测到（配合射线检测）
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player" || PlayerManager.p1_Role.IsInteracting == true) return;
        PlayerManager.p1_Role.OnEnterInteractArea(gameObject);
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        //if (collision.gameObject.layer != LayerMask.NameToLayer("Player")) return;
        if (collision.gameObject.tag != "Player") return;
        PlayerManager.p1_Role.ExitInteractArea(gameObject);
    }
}
