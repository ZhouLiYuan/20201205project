using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//这个协程用法讲解怎么样https://blog.csdn.net/qq_15020543/article/details/82701551（先暂时不管迭代器）

public class CirclePanel : MonoBehaviour
{
    //不能直接把脚本挂在canvas上，由于容易被连带误删，UI组件上一般不挂脚本
    //[SerializeField] private RectTransform panelRect;

    [SerializeField] private int elementCount; 
    [SerializeField] private Camera screenCam;

    [SerializeField] private GameObject circlePanel;
    /// <summary> 
    /// 用一个element image 旋转生成剩余的元素 图像
    /// </summary>
    [SerializeField] private GameObject elementPrefab;
    [SerializeField] private Transform elementParent;
    private List<Image> m_images;
    private RectTransform circlePanelRect;
    //选中时的协程
    private Coroutine OnSelectCoroutine;
    
    //初始值赋-1（在计算index时不会被算到，-1在这是用作为状态值（没有被选中这个状态）来使用  ）
    private int m_index = -1;
    //状态较多的情况用 枚举 和 switch 来做状态机（switch限制状态条件会比多重if更严谨 可读性更高）
    //四个状态，1待机 2选择中 3没选中 4选中了（3 4像是分支）
    private enum State { Standby,Selecting,NoSelect,OnSelected}
    private State m_state = State.Standby;


    private void Start()
    {
       
        //InitializingElements();
        CreatingElements();
        

        //circlePanelRect = this.GetComponent<RectTransform>();

        
    }

    private void Update()

    //ScreenPointToLocalPointInRectangle
    //官网地址https://docs.unity3d.com/ScriptReference/RectTransformUtility.ScreenPointToLocalPointInRectangle.html
    {
        HandleInput();
    }

    /// <summary>
    ///新方法：根据元素个数需求 动态创建元素Gobj
    /// </summary>
    private void CreatingElements() 
    {
        //mono里什么情况下能用new操作符，还是说其实可以用new，只是 new的实例 和作为 mono组件的实例 有些不同，不会依附在特定的实例Gobj上
        //构造函数初始化集合容量
        //这里的集合主要用作中转一些数据处理，所以可以用new？
        m_images = new List<Image>(elementCount);
        for (int i = 0; i < elementCount; i++)
        {
            //生成元素
            var objTemp = Instantiate(elementPrefab, elementParent);

            //为rotation部分赋值
            objTemp.GetComponent<RectTransform>().rotation = Quaternion.Euler(0f, 0f, i * (360f / elementCount));
            //调用集合添加元素方法
            m_images.Add(objTemp.GetComponent<Image>());

            //为什么要做这一步？
            elementPrefab.SetActive(false);

        }

    }

    /// <summary>
    /// 旧方法，实现把所有element创建好，而不是根据需求动态程序创建
    /// </summary>
    private void InitializingElements()
    {
        for (int i = 0; i < m_images.Count; i++)
        {
            //需要获取的是所有element Gobj的image，所以要用image集合
            //根据自己对canvas下image命名方式来找到各个GOBJ上的Image组件实例，并赋值给集合中的每个元素
            m_images[i] = GameObject.Find($"Image({i})").GetComponent<Image>();
        }
    }





    private void HandleInput()
    {
        switch (m_state)
        {
            //待机状态下，按下O，进入选择状态
            case State.Standby:
                if (Input.GetKey(KeyCode.O)){ m_state = State.Selecting; }
                break;

               //有效的选择，进入 已选择状态
            case State.Selecting:
                ChoosingElement();
                //松开按键的时候
                if (!Input.GetKey(KeyCode.O))
                //m_state = m_index == -1 这种连等是什么写法？-1不是 没有选中 状态吗？这里返回的状态是不是写反了
                { m_state = m_index == -1 ? State.OnSelected : State.OnSelected; }
                break;

                //无效选择，返回待机状态
            case State.NoSelect:
                m_state = State.Standby;
                break;
                //已经选择并执行对应功能（协程，UI变色）
            case State.OnSelected:
                StartCoroutine(SelectOne());
                break;
            default:
                break;
        }


        //if (m_state == State.Standby && Input.GetKey(KeyCode.O)) { ChooseElement(); }

        
        //else
        //{
        //    //协程调用(只有调用过ChooseElement()方法，m_index 才会变成-1以外的值，才会在按下按键并松开的时候满足，1没有按下按键，2 m_index不等于-1 这两个条件)
        //    //if (m_index != -1) { StartCoroutine(SelectOne()); }
        //    //状态为被选中时
        //    if (m_state == State.OnSelected) { StartCoroutine(SelectOne()); }
        //}

    }

    private void ChoosingElement()
    {
        //激活面板
        circlePanelRect.gameObject.SetActive(true);
        //确认screen的点在Rect中？这个方法会受Rect的pivot（作为中心基准）的影响
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(circlePanelRect, Input.mousePosition, screenCam, out Vector2 localPoint))
        {
            //用于判断位置的角度
            float m_degree = ComputeMousePosition(localPoint);

            

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

                    m_state = State.Selecting;
                    return;

                }

                else { m_index = -1; Debug.Log("无效选择范围"); }//这样就可以让区间 范围外的位置选择无效化
              
            }


        }
    }


    /// <summary>
    /// 需要传入本地坐标，返回角度
    /// </summary>
    /// <param name="v2"></param>
    private float ComputeMousePosition(Vector2 v2)
    {
        //默认中心pivot是canvas的pivot
        Debug.Log("设置锚点为" + v2);
        //弧度
        var radian = Mathf.Atan2(v2.y, v2.x);
        //角度(Rad2Deg是像3.14一样弧度换算角度的一个常数)
        float degree = Mathf.Rad2Deg * radian;
        Debug.Log($"输出锚点={v2}弧度={radian}角度={degree}");
        return degree;
    }


    /// <summary>
    ///  迭代器目前就记得  做延时wait效果就行 (C#图解讲的不全)
    /// </summary>
    /// <returns></returns>
    private IEnumerator SelectOne()
    {
        var m_image = m_images[m_index];
        //设置被选中时的高亮颜色
        m_image.color = Color.green;

        //开始执行开启（调用迭代器）协程 的地方 ，执行之后代码的同时，等待下述 秒数(2f)后 继续执行迭代器中的内容
        yield return new WaitForSeconds(2f);
        //也就是鼠标停留再image上两秒后由绿色变成红色？
        m_image.color = Color.red;
        //重置m_index
        m_index = -1;

        //关闭圆盘 并 重置状态
        circlePanelRect.gameObject.SetActive(false);
        m_state = State.Standby;
    }

}
