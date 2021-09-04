using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 名字可以替换成投射物  教程地址https://www.youtube.com/watch?v=wkKsl1Mfp5M&t=0s 16分50秒有讲解EffectLine的设置方法
/// </summary>
public class Gun : MonoBehaviour
{
    /// <summary>
    /// 投射物发射点
    /// </summary>
    public Transform startPoint;
    public GameObject bulletPrefab;
    public GameObject laserPrefab;
    public GameObject l_hitEffect;

    public LineRenderer m_lineRenderer;


    private float atkInterval = 0;
    public float atkIntervalVolume;
   
    /// <summary>
    /// 镭射枪的伤害数值
    /// </summary>
    public int l_atkValue;


    private void Update()
    {
        //之后可以放入inputHandler
        if (Input.GetKey(KeyCode.I)) { ShootBullet(); }
        if (Input.GetKey(KeyCode.P)) { StartCoroutine( ShootLaser()); }
    }

    void ShootBullet() 
    {  
        Instantiate(bulletPrefab, startPoint.position, startPoint.rotation);   
        //具体伤害数值的实现交给 挂载再子弹上的脚本实现
    }

    IEnumerator ShootLaser() 
    {

        //hitInfo被击中物的信息
        RaycastHit2D hitInfo = Physics2D.Raycast(startPoint.position, startPoint.right);

        if (hitInfo)
        {
            Debug.Log(hitInfo.transform.name);

            //为什么找的是Enemy游戏组件，而不是敌人GOBJ本身？
            BaseEnemy enemy = hitInfo.transform.GetComponent<BaseEnemy>();
            if (enemy!= null)
            {
                GetComponent<EnemyGetDamage>().TakeDamage(l_atkValue);
            }
            //生成被镭射击中的特效
            Instantiate(l_hitEffect, hitInfo.point, Quaternion.identity);

            //setPosition，两点确定一线，点之间有先后顺序，第一个参数是 绘制顺序 第二个是点的位置
            m_lineRenderer.SetPosition(0, startPoint.position);
            //在击中敌人的情况下，第二个点是敌人
            m_lineRenderer.SetPosition(1, hitInfo.point);

        }
        else 
        {
            m_lineRenderer.SetPosition(0, startPoint.position);
            //在没有击中敌人的情况下，第二个点是无限远
            m_lineRenderer.SetPosition(1, startPoint.position + startPoint.right*100);
        }


        m_lineRenderer.enabled = true;
        //用协程来延时射线的消失
        //延时0.02秒
        yield return 0.02;
        m_lineRenderer.enabled = false;
    }



}
