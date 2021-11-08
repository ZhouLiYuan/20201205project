using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor.Animations;

//和自己实现的FSM配合使用
public static class AnimTool
{
    //animator是需要信息的对象（runtimeAnimatorController.animationClips似乎是不分层都可以拿到的）
    public static AnimationClip[] GetAllClip(Animator animator)
    {
        AnimationClip[] result = animator.runtimeAnimatorController.animationClips;
        return result;
    }
    //找到当前Clip 
    public static AnimationClip GetClipByName(Animator animator, string animClipName)
    {
        foreach (AnimationClip clip in GetAllClip(animator)) { if (clip.name == animClipName) return clip; }
        return null;
    }

    //目前没啥用，以后卡时机可能有用的代码，教程https://www.jianshu.com/p/0873379253d2
    // 代码来源https://forum.unity.com/threads/getting-the-current-frame-of-an-animation-clip.376561/
    public static int GetAnimFrame(Animator animator, string animClipName, int animLayer = 0)
    {
        int frame;
        var clip = GetClipByName(animator, animClipName);
        var clipInfos = animator.GetCurrentAnimatorClipInfo(animLayer);
        if (clipInfos.Length != 0) { return frame = (int)(clipInfos[0].weight * (clip.length * clip.frameRate)); }
        else
        {
            Debug.LogError($"{clip}帧数为0");
            return 0;
        }
    }

    //获取对应layer的state
    public static ChildAnimatorState[] GetLayerStates(Animator animator, int layerIndex = 0)
    {
        var runtimeController = animator.runtimeAnimatorController;
        var layer = ((AnimatorController)runtimeController).layers[0];
        var stateMachine = layer.stateMachine;  //获取层状态机
        return stateMachine.states;
    }

    //statesOfLayer中找到对应stateName的状态
    public static AnimatorState GetStateByName(string stateName, ChildAnimatorState[] statesOfLayer)
    {
        foreach (var childAnimatorState in statesOfLayer) { if (childAnimatorState.state.name == stateName) return childAnimatorState.state; }
        Debug.LogError($"无法获取{stateName}的AnimatorState");
        return default;
    }


    //希望能像71置换动态贴图一样，在非游戏运行时一键设置

    //官方https://docs.unity3d.com/cn/2019.3/ScriptReference/AnimatorOverrideController.html
    //用AnimatorOverrideController进行动画clip重写，然后赋值给runtimeAnimatorController，
    //不能直接改RuntimeAnimatorController?(有点像rigidbody一些属性赋值)
    public static void SetLayerStates<TStateMachineBehaviour>(Animator animator, int layerIndex = 0) where TStateMachineBehaviour : StateMachineBehaviour
    {
        var animatorStates = GetLayerStates(animator, layerIndex);//暂时默认animator只有一层
        var clips = GetAllClip(animator);


        for (int i = 0; i < animatorStates.Length; i++)
        {
            var state = GetStateByName(clips[i].name, animatorStates); //原则上要让State名和AnimClip名一致（因为没有获取指定名字state的API，所以必须历遍）
            state.motion = ResourcesLoader.LoadAnimClip(animator.name, clips[i].name); ;//改Motion
            state.AddStateMachineBehaviour<TStateMachineBehaviour>();
            //文档https://docs.unity3d.com/cn/2019.4/ScriptReference/Animations.AnimatorState.AddStateMachineBehaviour.html
                                                                                        //FileStream fs;
                                                                                        //if (!File.Exists(AnimAssetPath)) fs = File.OpenRead(AnimAssetPath);
                                                                                        //FileStream fs = FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
                                                                                        //state.AddStateMachineBehaviour
                                                                                        //有没有办法根据animator.name获取对应的命名空间，根据state名自动添加对应的StateMachineBehaviour？
        }

        //var overrideController = new AnimatorOverrideController(); //新建重写的controller
        //overrideController.runtimeAnimatorController = animator.runtimeAnimatorController;
        //for (int i = 0; i < clips.Length; ++i)
        //{
        //    overrideController[clips[i].name] = clips[i]; //逐个覆盖原本的clip
        //}
        //animator.runtimeAnimatorController = overrideController; //重新赋值原本的runtimeAnimator
    }
}