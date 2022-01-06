using System;
using UnityEngine;

namespace BordlessFramework.Utility
{
    /// <summary>
    /// 显示所有子物体的名称和到父物体的连线，选中父物体时显示所有子物体的Position Handle
    /// </summary>
    public class MultiPositionHandleDrawer : MonoBehaviour
    {
#if UNITY_EDITOR

        [SerializeField] private bool isShowLineFromParent = false;
        [SerializeField] private bool isShowLine = true;
        [SerializeField] private bool isShowName = true;
        [SerializeField] private GUIStyle guiStyle;
        [SerializeField] private bool isSetAsGlobal = true;
        [SerializeField] private Color parentColor = Color.red;
        [SerializeField] private Color childColor = Color.cyan;
        private static Color globalParentColor;
        private static Color globalChildColor;

        private void OnDrawGizmos()
        {

            if (isShowName) UnityEditor.Handles.Label(transform.position, $"<b><color=#{ColorUtility.ToHtmlStringRGBA(isSetAsGlobal ? globalParentColor : parentColor)}>parent:</color></b> {transform.name}", guiStyle);

            for (int i = 0; i < transform.childCount; i++)
            {
                var t = transform.GetChild(i);
                if (isShowLineFromParent) Gizmos.DrawLine(transform.position, t.position);
                if (isShowName) UnityEditor.Handles.Label(t.position, $"<b><color=#{ColorUtility.ToHtmlStringRGBA(isSetAsGlobal ? globalChildColor : childColor)}>child:</color></b>: {t.name}", guiStyle);

                if (isShowLine)
                {
                    Transform next = i == transform.childCount - 1 ? transform.GetChild(0) : transform.GetChild(i + 1);
                    Gizmos.DrawLine(next.position, t.position);
                }
            }
        }

        private void OnValidate()
        {
            if (isSetAsGlobal)
            {
                if (globalParentColor != parentColor)
                {
                    globalParentColor = parentColor;
                    UnityEditor.EditorPrefs.SetString(nameof(globalParentColor), $"{globalParentColor.r},{globalParentColor.g},{globalParentColor.b},{globalParentColor.a}");
                }
                string[] colorComponent = UnityEditor.EditorPrefs.GetString(nameof(globalParentColor)).Split(',');
                globalParentColor = new Color(float.Parse(colorComponent[0]), float.Parse(colorComponent[1]), float.Parse(colorComponent[2]), float.Parse(colorComponent[3]));

                if (globalChildColor != childColor)
                {
                    globalChildColor = childColor;
                    UnityEditor.EditorPrefs.SetString(nameof(globalChildColor), $"{globalChildColor.r},{globalChildColor.g},{globalChildColor.b},{globalChildColor.a}");
                }

                string[] colorComponent2 = UnityEditor.EditorPrefs.GetString(nameof(globalChildColor)).Split(',');
                globalChildColor = new Color(float.Parse(colorComponent2[0]), float.Parse(colorComponent2[1]), float.Parse(colorComponent2[2]), float.Parse(colorComponent2[3]));
            }

            if (guiStyle == null)
            {
                guiStyle = new GUIStyle();
                guiStyle.richText = true;
                guiStyle.normal.textColor = Color.green;
                guiStyle.fontSize = 14;
                guiStyle.alignment = TextAnchor.MiddleCenter;
            }
        }
#endif
    }
}