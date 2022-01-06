#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace BordlessFramework.Utility
{
    [CustomEditor(typeof(MultiPositionHandleDrawer), true), CanEditMultipleObjects]
    public class MultiPositionHandleDrawerEditor : Editor
    {
        protected virtual void OnSceneGUI()
        {
            MultiPositionHandleDrawer example = (MultiPositionHandleDrawer)target;
            var children = example.transform.GetComponentsInChildren<Transform>();
            Undo.RecordObjects(children, $"ModifyMultiPositionHandleDrawer");
            EditorGUI.BeginChangeCheck();
            for (int i = 0; i < children.Length; i++)
            {
                var t = children[i];
                Vector3 newTargetPosition = Handles.PositionHandle(t.position, Quaternion.identity);
                t.position = newTargetPosition;
            }
        }
    }
}

#endif