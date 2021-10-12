using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : SingletonManager<NPC>
{
    //public static GameObject NPCList;
    //public static Transform NPCListTransform;

    //public static Dictionary<string, NPC> nameDic = new Dictionary<string, NPC>();


    //static NPCManager()
    //{
    //    NPCList = new GameObject("NPCList");
    //    NPCListTransform = NPCList.transform;
    //    NPCListTransform.position = Vector3.zero;
    //    NPCListTransform.rotation = new Quaternion(0, 0, 0, 0);
    //}


    //public static TNPC SpawnNPC<TNPC>(string prefabName, Vector3 bornPoint) where TNPC : NPC, new()
    //{
    //    Quaternion rotation = new Quaternion(0, 0, 0, 0);
    //    var prefab = ResourcesLoader.LoadNPCPrefab(prefabName);
    //    var obj = Object.Instantiate(prefab, bornPoint, rotation, NPCListTransform);
    //    TNPC npc = new TNPC();
    //    //初始化脚本component变量
    //    npc.Init(obj);
    //    return npc;
    //}

    //public static NPC GetNPCByName(string name)
    //{
    //    if (nameDic.TryGetValue(name, out var NPC))
    //    {
    //        return NPC;
    //    }
    //    return null;
    //}

}
