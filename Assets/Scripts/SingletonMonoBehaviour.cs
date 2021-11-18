using UnityEngine;

//单例mono
public class SingletonMonoBehaviour<T> : MonoBehaviour where T :MonoBehaviour
{
    static T s_instance;
    //全局静态字段随时获得mono单例
    public static T instance 
    {
        get 
        {
            if (s_instance == null)
            {
                s_instance = FindObjectOfType<T>();//前提：场景中一定存在一个MONO T
             
            }
            return s_instance;
        }
    }

    //原来Unity自带方法也可以重新定义为虚方法
    protected virtual void Awake() 
    {
        //if (s_instance == null) gameObject.AddComponent<T>();//保险起见没有的话应该Add一个？

        var objs = FindObjectsOfType<T>();
        if (objs.Length > 1)
        {
            Cleanup(objs);
        }
    }

    private void Cleanup(T[] list) 
    {
        for (int i = 1; i < list.Length; i++) 
        {
            Destroy(list[i].gameObject);//确保场景中只有一个T，多余的全部清除
        }
    }

    /// <summary>
    /// 加载新场景时也不要破坏当前单例 所在的Gobj
    /// </summary>
    protected void DontDestroy() 
    {
        DontDestroyOnLoad(gameObject);
    }

}
