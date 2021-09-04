using System;
using System.Collections.Generic;
using UnityEngine;

public static class ResourcesLoader
{
    public static DialogueData[] LoadDialogue(int storyId)
    {
        var json = AssetModule.LoadAsset<TextAsset>($"dialogueSample").text;
        var dialogueDatas = Newtonsoft.Json.JsonConvert.DeserializeObject<DialogueData[]>(json);
        List<DialogueData> results = new List<DialogueData>();
        for (int i = 0; i < dialogueDatas.Length; i++)
        {
            if (dialogueDatas[i].storyId != storyId) continue;
            results.Add(dialogueDatas[i]);
        }
        return results.ToArray();
    }

    public static void LoadPrefab(string name)
    {
        AssetModule.LoadAsset<GameObject>($"Prefab/{name}");
    }
}
