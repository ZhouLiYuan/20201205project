using UnityEngine;

public class PlayerInteractDetect : MonoBehaviour
{
    //目前直接在对应可交互Gobj的component上配置好
    public InteractableData data;
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Player")) return;
        //InteractableManager.Init<>
        GlobalEvent.EnterInteractArea(data);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Player")) return;
        GlobalEvent.ExitInteractArea(data);
    }
}
