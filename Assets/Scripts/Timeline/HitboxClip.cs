using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

// 如果不添加这个，会报UnityException: Invalid type的错误
// Apply this to a PlayableBehaviour class or field to indicate that it is not animatable.
[NotKeyable]
public class HitboxClip : PlayableAsset, ITimelineClipAsset
{
    public ClipCaps clipCaps => ClipCaps.None;
    [SerializeField] private Vector2[] rect;


    private void OnEnable()
    {
        UnityEditor.Selection.selectionChanged += Changed;
    }

    private void OnDisable()
    {
        UnityEditor.Selection.selectionChanged -= Changed;
    }

    private static void Changed()
    {

    }

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var instance = ScriptPlayable<HitboxBehaviour>.Create(graph);
        return instance;
    }
}