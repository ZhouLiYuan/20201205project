using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

// Apply this to a PlayableBehaviour class or field to indicate that it is not animatable.
[NotKeyable]
public class HitboxClip : PlayableAsset, ITimelineClipAsset
{
    public ClipCaps clipCaps => ClipCaps.All;
    [SerializeField] private Vector2[] rect;


    private void OnEnable()
    {
        //当前活动/所选项发生更改时触发的委托回调
        UnityEditor.Selection.selectionChanged += Changed;
    }

    private void OnDisable()
    {
        UnityEditor.Selection.selectionChanged -= Changed;
    }

    private static void Changed()//当改变 选择对象 时调用此方法
    {
       var activeObject = UnityEditor.Selection.activeObject as GameObject;//对激活物体做的一些操作
    }

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var instance = ScriptPlayable<HitboxBehaviour>.Create(graph);
        return instance;
    }
}