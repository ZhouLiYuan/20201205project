using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;


///// <summary>
///// 测试用Addressable加载
///// </summary>
//public class LoadAssetBundle : MonoBehaviour
//{

//    // Update is called once per frame
//    void Update()
//    {
//        //暂时用于触发资源加载
//        if (Input.GetKeyDown(KeyCode.Space))
//        {
//            LoadByAddressable();
//        }
//    }

//    private void LoadByAddressable() 
//    {
//        //模板
//        //Addressables.LoadAssetAsync<资源类型>(资源名字).Completed += handle  =>GetComponent<资源类型>().资源类型中封装的字段 = handle.Result;
//        //因为回调方法必须 和 事件Completed（泛型委托）的类型参数一致（也就是AsyncOperationHandle<Sprite>）
//        //可以跳转定义看Result
//        Addressables.LoadAssetAsync<Sprite>("Axe123").Completed += handle => GetComponent<SpriteRenderer>().sprite = handle.Result;
//        Debug.Log("用Addressable加载");
//    }


//}
