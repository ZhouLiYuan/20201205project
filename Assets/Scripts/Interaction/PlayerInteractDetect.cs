using UnityEngine;

public class PlayerInteractDetect : MonoBehaviour
{
    //目前直接在对应可交互Gobj的component上配置好
    public InteractableData data;
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Player")) return;

        SceneObjManager.InteractableEntities.Add(transform.gameObject);

        PlayerManager.m_Role.interactableController.EnterInteractArea(data);
        //GlobalEvent.EnterInteractArea(data);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Player")) return;
        PlayerManager.m_Role.interactableController.ExitInteractArea(data);

    }
}
