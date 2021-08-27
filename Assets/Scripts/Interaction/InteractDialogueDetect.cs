using UnityEngine;

//需要用到mono碰撞检测
public class InteractDialogueDetect:MonoBehaviour
{
    public InteractableDialogueData m_data;

    //碰撞范围是怎么实现的？
    //这个脚本应该是挂在敌人身上吧
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //碰撞到的Gobj所在layer编号 是否与 名为“Player”的图层编号相等？
        //如果编号不相等就退出方法 为什么不按正逻辑如果相等，就执行进入交互状态方法
        if (collision.gameObject.layer != LayerMask.NameToLayer("Player")) return;
        GlobalEvent.EnterInteractArea(m_data);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Player")) return;
        GlobalEvent.ExitInteractArea(m_data);
    }
}
