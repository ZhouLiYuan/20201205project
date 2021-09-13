using UnityEngine;
using Cinemachine;

public static class CameraManager
{
    /// <summary>
    /// 初始化相机
    /// </summary>
    public static void InitCamera(PlayerRole role)
    {
        //设置Main相机参数
        var mainCameraObj = new GameObject("MainCamera");
        var mainCamera = mainCameraObj.AddComponent<Camera>();
        mainCamera.clearFlags = CameraClearFlags.Skybox;
        mainCamera.tag = "MainCamera";
        mainCamera.orthographic = true;
        mainCamera.orthographicSize = 5.4f;

        //设置虚拟相机参数（目前只有跟随相机）
        var brain = mainCameraObj.AddComponent<CinemachineBrain>();
        var vcam = new GameObject("vcam_PlayerRole").AddComponent<CinemachineVirtualCamera>();
        vcam.AddCinemachineComponent<CinemachineTransposer>();
        vcam.Follow = role.Transform;
    }
}
