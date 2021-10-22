using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagManager
{
    public static string Player => "Player";
    public static string Enemy => "Enemy";

    //功能尚未检验
    public static string AddTag(string tagName)
    {
        for (int i = 0; i < UnityEditorInternal.InternalEditorUtility.tags.Length; i++)
        {
            if (UnityEditorInternal.InternalEditorUtility.tags[i].Contains(tagName)) return UnityEditorInternal.InternalEditorUtility.tags[i];
        }
        UnityEditorInternal.InternalEditorUtility.AddTag(tagName);
        return tagName;
    }
}


public class LayerManager
{
    public static string Player => "Player";
    public static string Enemy => "Enemy";
    public static string Attacker => "Attacker";
}