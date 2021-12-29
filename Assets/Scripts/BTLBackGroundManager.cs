using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//目前Stage 和 Background 暂时放在一起管理
//暂时不用SingletonManager
//BackGround的Z轴和显示是配置好的（不能动态配置），只需从gameObject上获取信息即可
public class BTLBackGroundManager
{
    //BG需要跟随的相机
    public static GameObject CameraGobj;
    private static Vector3 currentCamPos => CameraGobj.transform.position;
    private static Vector3 oldCamPos;//上一帧相机的位置

    //private static float stageLength;


    private static GameObject stageGobj;
    private static Transform stageTransform;

    public static List<BackGround> Items = new List<BackGround>();
    public static List<BackGround> FrontItems = new List<BackGround>();
    public static List<BackGround> MiddleItems = new List<BackGround>();
    public static List<BackGround> BackGroundtems = new List<BackGround>();

    public static void SpawnStage(int StageID = 0) 
    {
        //创建BTL舞台
        var stagePrefab = ResourcesLoader.LoadBtlStage();
        //覆盖父类字段
        stageGobj = Object.Instantiate(stagePrefab);
        stageTransform = stageGobj.transform;
    }

    public static void Init()
    {
        //CameraGobj = GameObject.Find("Main Camera");//注意：场景中Main Camera非激活会找不到
        CameraGobj = CameraManager.mainCameraGobj;

        var platformTransform = stageTransform.Find("Platform");
        //stageLength = platformTransform.GetComponent<SpriteRenderer>().bounds.size.x;//获得Stage横向活动范围长度

        var frontGobjsTransform = stageTransform.GetComponentsInChildren<Transform>(true).Where(obj => obj.name.Contains("Front"));
        var middleGobjsTransform = stageTransform.GetComponentsInChildren<Transform>(true).Where(obj => obj.name.Contains("Middle"));
        var backGroundGobjsTransform = stageTransform.GetComponentsInChildren<Transform>(true).Where(obj => obj.name.Contains("BackGround"));

        InitItemsList(frontGobjsTransform,FrontItems);
        InitItemsList(middleGobjsTransform, MiddleItems);
        InitItemsList(backGroundGobjsTransform, BackGroundtems);
    }


    public static void InitItemsList(IEnumerable<Transform> SceneObjsTransform,List<BackGround> targetList)
    {
        foreach (var Item in SceneObjsTransform)  
        {
            var bg = new BackGround(Item.gameObject);
            targetList.Add(bg);
        }
        Items.AddRange(targetList);
    }

    //处理所有背景跟随相机移动量
    public static void OnUpdate()
    {
        if (currentCamPos.x == oldCamPos.x) return;
        var camDeltaDistance = currentCamPos.x - oldCamPos.x;
        foreach (var Item in Items) { Item.OnUpdate(camDeltaDistance); }
        oldCamPos.x = currentCamPos.x;
    }
}
