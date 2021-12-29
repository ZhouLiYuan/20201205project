using UnityEngine;
using Cinemachine;
using Role.SelectableRole;

public static class CameraManager
{
    public static GameObject mainCameraGobj;
    public static Camera mainCamera;

    private static CinemachineVirtualCamera vcam;

    /// <summary>
    /// 初始化相机
    /// </summary>
    public static void InitCamera()
    {
        //设置Main相机参数
        mainCameraGobj = new GameObject("Main Camera");//不要漏空格
        mainCamera = mainCameraGobj.AddComponent<Camera>();
        mainCamera.clearFlags = CameraClearFlags.Depth;
        mainCamera.tag = "MainCamera";
        mainCamera.orthographic = true;
        mainCamera.orthographicSize = 5.4f;

        //设置虚拟相机参数（目前只有跟随相机）
        var brain = mainCameraGobj.AddComponent<CinemachineBrain>();
        vcam = new GameObject("vcam_PlayerRole").AddComponent<CinemachineVirtualCamera>();//加了这个组件后好像就没办法随心移动Camera了
        vcam.AddCinemachineComponent<CinemachineTransposer>();
    }
    //设置相机的追踪对象
    public static void SetAdvModeAim(Transform aim){vcam.Follow = aim;}

}

//相机跟随
//public Transform m_cam;

////作用 固定住UI一直面向屏幕
//void LateUpdate()
//{
//    //这里有点没看懂为什么要 +
//    transform.LookAt(transform.position + m_cam.forward);
//}


//public class FollowCamera : MonoBehaviour
//{
//    public Transform cameraTransform;
//    public Transform targetTransform;

//    [Range(0f, 3f)] 
//    public float cameraFollowSpeed = 1f;

//    void Update()
//    {
//        //fps每秒传输帧数 30fps 和 60fps 帧与帧之间的时间间隔是不一样的
//        //Time.deltaTime 当前帧与上一帧之间的时间 当fps不稳定时，Update帧与帧之间的时间间隔是不稳定的，deltaTime保证每秒时间移动量相同
//        float step = cameraFollowSpeed * Time.deltaTime;
//        cameraTransform.position = Vector3.MoveTowards(cameraTransform.position, targetTransform.position, step);

//        cameraTransform.position = new Vector3(cameraTransform.position.x, cameraTransform.position.y, -1f);
//        //加上f可减少重载时的隐式转化（和sorting layer类似）
//    }
//}

//视差相机的本质是 不同层以不同速度运动

//所以背景的每一层都需要 层级信息 Z轴信息