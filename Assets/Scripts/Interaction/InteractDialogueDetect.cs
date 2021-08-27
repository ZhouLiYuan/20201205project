using UnityEngine;

public class InteractDialogueDetect : MonoBehaviour
{
    public InteractableDialogueData data;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Player")) return;
        GlobalEvent.EnterInteractArea(data);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Player")) return;
        GlobalEvent.ExitInteractArea(data);
    }
}
