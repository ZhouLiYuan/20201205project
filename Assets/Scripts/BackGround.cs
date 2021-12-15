using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//越远的背景跟随相机速度越快
//如果是AdvBG，背景是可滚动复用的话，还得把背景滚动的逻辑也考虑进去，可参考6分30秒处https://www.youtube.com/watch?v=zit45k6CUMk
public class BackGround:Entity
{
    //相对相机移动量
    private float parallaxFactor;
    public float ParallaxFactor
    {
        get{ return parallaxFactor; }
        set{ parallaxFactor = value > 1 ? 1 : value; }
    }

    //private static Vector3 startPos;//记录初始位置
    private static float pictureLength;//Sprite横向长度

    public BackGround() : base() {    }

    public BackGround(GameObject obj) : base(obj)
    {
        //startPos = Transform.position;
        //pictureLength = Transform.GetComponent<SpriteRenderer>().bounds.size.x;
        ParallaxFactor = Mathf.Abs(Transform.position.z) * 0.005f;//因为z轴值目前都挺大的所以 x 0.005

        //先咋暂时乱给参数
        if (GameObject.name.Contains("Front")) { return; }
        if (GameObject.name.Contains("Middle")) 
        {
            ParallaxFactor -= 0.1f;
            return;
        }
        if (GameObject.name.Contains("BackGround"))
        {
            ParallaxFactor -= 0.2f;
            return;
        }
    }


    //跟随相机
    private void FollowCam(float CamDeltaDistance)
    {
        Vector3 newPos = Transform.localPosition;
        newPos.x += CamDeltaDistance * ParallaxFactor;
        Transform.localPosition = newPos;
    }

    public void OnUpdate(float camDeltaDistance) 
    {
        //var BGdeltaDistance = (Transform.position - startPos).sqrMagnitude;
        //if (BGdeltaDistance>Mathf.Sqrt(pictureLength)*0.25)
        FollowCam(camDeltaDistance);
    }

}
