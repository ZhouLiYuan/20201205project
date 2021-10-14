using UnityEngine;
using UnityEngine.UI;

//目前设计挂在NPC或者交互物的Top_Node上
public class PlayerInteractDetect : MonoBehaviour
{
    //角色正对着才会检测到（配合射线检测）
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player" || PlayerManager.m_Role.IsInteracting == true) return;
        PlayerManager.m_Role.OnEnterInteractArea(gameObject);
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        //if (collision.gameObject.layer != LayerMask.NameToLayer("Player")) return;
        if (collision.gameObject.tag != "Player") return;
        PlayerManager.m_Role.ExitInteractArea(gameObject);
    }
}
