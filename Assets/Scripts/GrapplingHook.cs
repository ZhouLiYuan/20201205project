using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    //钩子本身
    public GameObject hook;
    //hookHolder可以理解为玩家本身
    public GameObject hookHolder;
    //勾到的物体
    public GameObject hookedObj;

    public const float minDistance = 1;
    public float hookTravelSpeed;
    public float playerTravelSpeed;
    //钩锁最大长度
    public float maxDistance;

    //钩子和角色之间的距离
    private float currentDistance;
    public float returnTime = 2f;


    public static bool fired;
    public bool hooked;
    private bool grounded;

    public void Grappling()
    {

        //发射钩锁 并且确保钩锁在发出状态下不会重复发射
        if (Input.GetButtonDown("Fire1") && fired == false)
        {
            fired = true;

        }




        if (fired == true && hooked == false)
        {
            
            if (fired)
            {   //  把一个  叫做 LineRenderer 的 component的信息赋值给 rope引用变量
                LineRenderer rope = hook.GetComponent<LineRenderer>();
                rope.SetVertexCount(2);  
                rope.SetPosition(0, hookHolder.transform.position);
                rope.SetPosition(1, hook.transform.position);
                //这部分和渲染有关
            }

            //计算钩锁飞出后位置
            hook.transform.Translate(Vector3.right * Time.deltaTime * hookTravelSpeed);
            Vector3 hookDirection = new Vector3(0f, -1f, 0f);

            
            currentDistance = Vector3.Distance(hookHolder.transform.position, hook.transform.position);

            //超过钩锁长度（够不到）的时候
            if (currentDistance >= maxDistance)
                ReturnHook();
            
        }

        if (hooked == true && fired == true)
        {
            hook.transform.parent = hookedObj.transform;
            //重新配对父子关系，如果被勾中的是一个移动平台，那么可以持续获得钩中点的位移变化
            //备用方案：可以通过缓存钩中点的position，再让角色position数值不断往这个点变化
            hookHolder.transform.position = Vector3.MoveTowards(hookHolder.transform.position, hook.transform.position, Time.deltaTime * playerTravelSpeed);
            float distanceToHook = Vector3.Distance(hookHolder.transform.position, hook.transform.position);
            //这里要重新声明一个distance作区分
            //currentDistance是 发射hook的时候 和hook之间的距离
            //distanceToHook则是勾中了后角色往hook移动时 和hook之间的距离

            //让玩家不受重力影响
            hookHolder.GetComponent<Rigidbody2D>().gravityScale = 0;

            if (distanceToHook < minDistance)
               
                //这个方法是为了最后收钩锁之前给一个向上的速度  方便角色从略微上空着陆
                if (grounded == false)
                {
                    hookHolder.transform.Translate(Vector3.up * Time.deltaTime * 10f);
                    
                }

            StartCoroutine(Climb());
            //协程需要继承自mono才能用
        }
        else
        {   //如果不满足上面if的条件，就把钩子返还给角色
            hook.transform.parent = hookHolder.transform;
            hookHolder.GetComponent<Rigidbody2D>().gravityScale = 1;
        }
    }
    IEnumerator Climb()
    {
        yield return new WaitForSeconds(0.1f);
        //WaitForSeconds和协程搭配的关键字 看C#详解
        ReturnHook();
    }


    IEnumerator ReturnHook()
    {
        //确保旋转轴一致
        hook.transform.rotation = hookHolder.transform.rotation;
        float currentTime = returnTime;

        while (0f < currentTime)
        { //这里是模拟时间流逝？
            currentTime -= Time.deltaTime;
            hook.transform.position = Vector3.MoveTowards(hook.transform.position, hookHolder.transform.position, Time.deltaTime * hookTravelSpeed);
            //hook的收回速度需要手动设定
            yield return new WaitForEndOfFrame();//迭代器里到底具体做了什么事
        }

        //hook.transform.position = hookHolder.transform.position;

        //归位钩锁？
        fired = false;
        hooked = false;

        LineRenderer rope = hook.GetComponent<LineRenderer>();
        rope.SetVertexCount(0);

    }
}    

