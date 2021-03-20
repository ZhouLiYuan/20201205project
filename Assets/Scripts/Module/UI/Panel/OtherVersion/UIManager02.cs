using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager02
{
    /// <summary>
    /// 配合UIInfo查询对应的Gobj
    /// </summary>
    private Dictionary<UIInfo, GameObject> dicUI;

    public UIManager02()
    {
        dicUI = new Dictionary<UIInfo, GameObject>();
    }

    /// <summary>
    /// 获取一个 UI 实例
    /// 有返回值的方法，一般都需要判断返回的变量对应值是否为null
    /// </summary>
    /// <param name="info"></param>
    /// <returns></returns>
    public GameObject GetSingleUI(UIInfo info)
    {
        GameObject parent = GameObject.Find("Canvas");
        //画布非空判断，能不能写成?.形式？
        if (!parent)
        {
            Debug.LogError("淦！你是不是没在场景中放canvas！？");
            return null;
        }
        //因为字典只要有key 哪怕没有指定value 也会有个映射null 
        //所以才必须用ContainsKey来判断 映射（value）是否非空？
        //dicUI[key]是获取对应Value？
        if (dicUI.ContainsKey(info)) { return dicUI[info]; }

        //如果没有value，就创建一个，再返回
        GameObject uiGobj = GameObject.Instantiate(Resources.Load<GameObject>(info.Path), parent.transform);
        uiGobj.name = info.Name;
        //记得生成的Gobj value也给字典加上
        dicUI.Add(info, uiGobj);
        return uiGobj;
    }

    /// <summary>
    /// 销毁 一个UI对象
    /// </summary>
    /// <param name="info"></param>
    public void DestroyUI(UIInfo info)
    {
        GameObject.Destroy(dicUI[info]);
        dicUI.Remove(info);
    }
}
