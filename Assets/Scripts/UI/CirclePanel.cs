using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//这个协程用法讲解怎么样https://blog.csdn.net/qq_15020543/article/details/82701551（先暂时不管迭代器）

public class CirclePanel : MonoBehaviour
{
    //直接把脚本挂在canvas上
    //[SerializeField] private RectTransform panelRect;
    [SerializeField] private int elementCount;
    [SerializeField] private Camera screenCam;

    private RectTransform circlePanelRect;
    
    //为什么初始值赋为-1（因为永远在计算index时用不上，并且可以用于重置状态？）
    private int m_index = -1;

    private void Start()
    {
        circlePanelRect = this.GetComponent<RectTransform>();
    }

    private void Update()

    //ScreenPointToLocalPointInRectangle
    //官网地址https://docs.unity3d.com/ScriptReference/RectTransformUtility.ScreenPointToLocalPointInRectangle.html
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetKey(KeyCode.O)) { ChooseElement(); }

        //协程调用(只有调用过ChooseElement()方法，m_index 才会变成-1以外的值，才会在按下按键并松开的时候满足，1没有按下按键，2 m_index不等于-1 这两个条件？)
        else
        {
            if (m_index != -1) { StartCoroutine(SelectOne()); }
        }

    }

    private void ChooseElement()
    {
        //激活面板
        circlePanelRect.gameObject.SetActive(true);
        //确认screen的点在Rect中？这个方法会受Rect的pivot（作为中心基准）的影响
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(circlePanelRect, Input.mousePosition, screenCam, out Vector2 outVec))
        {
            //用于判断位置的角度
            float m_degree;

            ComputeMousePosition(outVec, out m_degree);

            //角度是1°=1f吗？
            float elementDegree = 360 / elementCount;

            float currentDegree = -180f / elementCount;
            float nextElementDegree = currentDegree + elementDegree;

            //每次都按顺序挨个检查if条件性能不会很差吗？
            for (int i = 0; i < elementCount; i++, currentDegree += elementDegree)
            {
                Debug.Log($"第{i}个，区间：{currentDegree}~{nextElementDegree}");
                if (currentDegree < m_degree && m_degree < nextElementDegree)
                {
                    Debug.Log($"选中  第{i}个，范围：{currentDegree}~{nextElementDegree}");
                    m_index = i;
                }
                //这种==写法有没有问题
                //else if (m_degree == currentDegree || m_degree == nextElementDegree) { }
                else { m_index = -1; Debug.Log("无效选择范围"); }//这样就可以让区间 范围外的位置选择无效化？
            }


        }
    }

    private void ComputeMousePosition(Vector2 v2, out float degree)
    {
        //默认中心pivot是canvas的pivot？
        Debug.Log("设置锚点为" + v2);
        //弧度
        var radian = Mathf.Atan2(v2.y, v2.x);
        //角度(Rad2Deg是像3.14一样弧度换算角度的一个常数)
        degree = Mathf.Rad2Deg * radian;
        Debug.Log($"输出锚点={v2}弧度={radian}角度={degree}");
    }


    //迭代器目前就记得可以用来 做延时wait效果就行 (C#图解讲的不全)
    private IEnumerator SelectOne()
    {
        var m_image = GameObject.Find($"根据自己对canvas下image命名方式来找到需要修改的GOBJ{m_index}").GetComponent<Image>();
        //设置被选中时的高亮颜色
         m_image.color= Color.green;

        //开始执行开启（调用迭代器）协程 的地方 ，执行之后代码的同时，等待下述 秒数(2f)后 继续执行迭代器中的内容
        yield return new WaitForSeconds(2f);
        //也就是鼠标停留再image上两秒后由绿色变成红色？
        m_image.color = Color.red;
        //重置m_index
        m_index = -1;

        //关闭圆盘
        circlePanelRect.gameObject.SetActive(false);
    }

}
