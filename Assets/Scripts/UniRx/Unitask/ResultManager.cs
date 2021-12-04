using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using TMPro;//71也有用
using UniRx;//71也有用
using Cysharp.Threading.Tasks;
using App.Sound;


//UniRx 在ver7.0后分离了两个模块UniRx和UniTask，UniRx.Async命名空间不复存在
//(取而代之需要引用Cysharp.Threading.Tasks命名空间) 详见https://increment-log.com/unirx-async-separated-unitask/
//API查找https://www.jianshu.com/p/94b857be40ec



//关于异步较好的讲解https://www.cnblogs.com/zhaoshujie/p/11192036.html
//知识点：1 从await后 同方法体内的代码在新开的线程上执行，然后跳出方法体后续代码会在异步方法执行的同时执行
//             2 如果需要用到异步方法的返回值 或者 处理结果 那么要小心锁死或者空引用
//异步更像是同时进行，协程则可暂时理解为是 加了阻塞功能 版本的异步， 在执行外部代码的同时，满足“yield return + 等待条件”中的条件后再执行后续代码
//异步和回调关联强


//每个关卡结束时的评价面板，暂时模仿鬼泣或者恶魔城( 应该弄一个ResultPanel)，目前暂定ResultManager挂在ResultPanel上，为button提供一些点击回调事件
//成功闯关时才会出现的面板ResultPanel
//与ScoreManager强关联
public class ResultManager:SingletonMonoBehaviour<ResultManager>
{
    //---------------------<UI配置>--------------------
    [SerializeField] TextMeshProUGUI m_scoreText;//后期应该全部写死在脚本里（先暂时用手动拖拽方式实现）
    [SerializeField] TextMeshProUGUI m_bestScoreText;
    //当Inspector中 需要多个变量又不用单个元素变量名时（适用于 集合内元素 处理比较统一的情况）
    [SerializeField] Image[] m_icons;//面板上的UI图像

    //-------------------<ResultPanel>---------------------
    //动画用(目前暂定从下网上滚)
    [SerializeField] RectTransform m_resultPanelTransform;
    [SerializeField] float m_scrollSpeed = 30f;//满版滚动速度

    [SerializeField] float m_feedoutTime = 0.5f;//场景切换速度
    IDisposable m_fadeout;//接口的引用 貌似 不能作 未被赋值的本地变量（从头到尾没看懂这个引用有什么用）
    float m_time = 0;

    //-------------------<Unity生命周期函数>---------------------
    void Start() 
    {
        gameObject.SetActive(false);
    }


    //-------------------<Button对应的回调方法>---------------------

    //异步不是多线程(游戏不存在多线程)多线程只是异步的一种手段http://www.choupangxia.com/2021/02/15/async/
    public async UniTask OnResultAsync() 
    {
        gameObject.SetActive(true);
        SoundManager.Instance.StopBgm();

        int score = ScoreManager.Instance.TotalScore.Value;//总分
        m_scoreText.text = $"{score:#,0}";//后面的 :#,0 有什么作用？


        int bestScore = ScoreManager.Instance.LoadScore();
        m_bestScoreText.text = $"{bestScore:#,0}";

        await ScrollUp();//在异步的同时，异步所在的新线程 while后的代码或下一轮的循环 都在每帧末执行
    }

    IEnumerator ScrollUp()
    {
        while (m_resultPanelTransform.position.y < 0) //村上的默认设置中Pos Y=-1920(隔了一整个游戏屏幕)
        {
            m_resultPanelTransform.Translate(0, Time.deltaTime * m_scrollSpeed, 0);
            yield return new WaitForEndOfFrame();//暂时跳出ScrollUp()方法体外继续执行方法体外的代码，延时（阻塞）到每帧末才处理方法体内yield return的后续内容
        }
        m_resultPanelTransform.position = Vector3.zero;
    }

    public void OnContinue ()
    {
        SceneChange(m_feedoutTime);
    }

    public void OnBack()
    {
        SceneChange(m_feedoutTime);
    }

    //这个方法本身也没太看懂
    public void SceneChange(float second) 
    {
        m_fadeout?.Dispose();//接口引用非空 配置？

        var curve = new AnimationCurve();
        curve.AddKey(0, 1);//API k帧
        curve.AddKey(second, 0);

        m_time = 0f;//重置计时
        //FrameInterval和IntervalFrame不是一个东西
        //实现接口引用
        m_fadeout = Observable
            .IntervalFrame(1)
            .TakeWhile(_ => curve.Evaluate(m_time) > 0)//当参数为false时停止发射数据  ("_"是匿名方法的参数）
            .Subscribe(_ =>//订阅了两个匿名回调方法 1,改变UI不透明度
            {
                m_time += Time.deltaTime;
                var color = new Color(1, 1, 1, curve.Evaluate(m_time));//用 曲线key(即time)对应的value变化 来控制透明度

                foreach (var icon in m_icons)
                {
                    icon.color *= color;//*=目的是为了只改变 透明度参数（icon颜色默认透明度也是1）
                }
                m_scoreText.color *= color;
                m_bestScoreText.color *= color;
            }
            ,
            () =>
            {
                m_fadeout.Dispose();//非空检测开头做过了（但依然不知道是什么时候赋值的。。。。）
                m_fadeout = null;

                SceneManager.LoadScene(SceneManager.GetActiveScene().name);//重新加载当前场景（场景加载可以因需求而异）
            })
            .AddTo(this);//貌似是把组件实例配置到重新加载的场景中？

    }

}
