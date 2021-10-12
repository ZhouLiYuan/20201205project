using UnityEngine;


//目前设计挂在NPC或者交互物的Top_Node上
public class PlayerInteractDetect : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //追加功能，角色正对着才会出现可交互outline高亮（配合射线检测）
        //if (collision.gameObject.layer != LayerMask.NameToLayer("Player") || PlayerManager.m_Role.IsInteracting == true) return;
        if (collision.gameObject.tag != "Player" || PlayerManager.m_Role.IsInteracting == true) return;
        //Player进入交互区域
        PlayerManager.m_Role.isInInteractArea = true;
        
        Debug.Log($"进入{transform.name}的交互区域");
        PlayerManager.m_Role.GobjsInInteractArea.Add(this.gameObject);    
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //if (collision.gameObject.layer != LayerMask.NameToLayer("Player")) return;
        if (collision.gameObject.tag != "Player") return;
        Debug.Log($"离开{transform.name}的交互区域");
        PlayerManager.m_Role.GobjsInInteractArea.Remove(this.gameObject);
        if (PlayerManager.m_Role.GobjsInInteractArea == null)  PlayerManager.m_Role.isInInteractArea = false;
        //PlayerManager.m_Role.ExitInteractArea(data);
    }
}
