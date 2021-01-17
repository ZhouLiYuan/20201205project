using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    //钩锁持有者（player）
    [SerializeField] private Transform owner;

    enum HookState { Standby, HookToTarget, CharacterToTarget }
    //声明枚举变量，设置初始状态
    [SerializeField] private HookState state = HookState.Standby;

    [SerializeField] private float hookToTargetSpeed = 2f;
    [SerializeField] private float distanceThreshold = 0.8f;
    [SerializeField] private float finalJumpDistance = 1.5f;
    [SerializeField] private float finalJumpUp =5f;


    //结构属于值类型，所以会有默认初始值0
    private Vector3 targetPosition;
    private bool finalJump = false;

    //用状态机重新实现钩锁功能(没有目标就无法发射版本)
    public void Shoot(Vector2 position)
    {
        //确保每一次发射钩锁都有finaljump的一次机会
        finalJump = false;
        //设置状态
        state = HookState.HookToTarget;
        targetPosition = position;
    }

    private void Update()
    {
        //不同状态标签下，不同的功能切换
        switch (state)
        {
            case HookState.Standby:
                break;

            case HookState.HookToTarget:

                //这是让hook的transform回归世界参考坐标系吗？没有这段代码不知道为什么character box就会一直飞停不下来
                this.transform.SetParent(null);
                this.transform.position += Time.deltaTime * (targetPosition - transform.position) * hookToTargetSpeed;
                //                                                                 提供方向   按这种方法岂不是离目标距离越远钩子的速度越快？
                if (Vector2.Distance(transform.position, targetPosition) < distanceThreshold)
                { 
                    state = HookState.CharacterToTarget;
                    //重力无效化 因为Transform继承自Component，所以可以用component里的字段gameObject
                    owner.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0f;
                }
                break;

            case HookState.CharacterToTarget:

                owner.position += Time.deltaTime * (transform.position - owner.position) * hookToTargetSpeed;

                //必须确保只有一次向上速度，所以需要加多一个布尔判断是否是最后一次跳跃（可以用取非）
                if (!finalJump &&Vector2.Distance(owner.position, transform.position) < finalJumpDistance)
                {
                    //最后再给一个向上的速度帮助character着陆
                    owner.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(owner.gameObject.GetComponent<Rigidbody2D>().velocity.x, finalJumpUp);
                    owner.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1f;
                    finalJump = true;
                }
                    if (Vector2.Distance(owner.position, transform.position) < distanceThreshold)
                {
                    state = HookState.Standby;
                    Debug.Log("回归standby");
                    //顺便让钩锁归位,不要忘记让角色的重力也恢复
                    transform.position = Vector3.zero;
                    owner.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1f;
                    this.transform.SetParent(owner, false);
                }
                break;

            default:
                break;
        }
    }



}
