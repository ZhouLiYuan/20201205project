using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ResourcesLoader
{
    //先从json读出 数组信息interactableDialogueDatas(本地变量)，再从完整的数组信息中根据storyId抽出一句，装进List类型的results（本地变量）
    //再把results转化为数组 作方法返回
    //19-21行不是多次一举吗为什么不直接返回interactableDialogueDatas[i]，是因为无法对比storyId信息吗（index i不一定和storyId一致）
    public static InteractableDialogueData[] LoadDialogue(int storyId) 
    {
        var json = AssetModule.LoadAsset<TextAsset>($"").text;
        //重点看一下这款的讲解(从json转到自定义的InteractableDialogueData[]类型？)
        var interactableDialogueDatas = Newtonsoft.Json.JsonConvert.DeserializeObject<InteractableDialogueData[]>(json);
        List<InteractableDialogueData> results = new List<InteractableDialogueData>();
        for (int i = 0; i < interactableDialogueDatas.Length; i++)
        {
            //只有当interactableDialogueData元素中的storyId和传入方法的参数相同时才会执行下面内容
            if (interactableDialogueDatas[i].storyId != storyId) continue;
            results.Add(interactableDialogueDatas[i]);
        }
        return results.ToArray();
    }

    public static void LoadPrefab(string name)
    {
        AssetModule.LoadAsset<GameObject>($"Prefab/{name}");
    }
}
