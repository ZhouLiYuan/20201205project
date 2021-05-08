using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestory : MonoBehaviour
{
    ParticleSystem effect;

    void Start()
    {
        effect = GetComponent<ParticleSystem>();
        StartCoroutine(Check());
    }

    IEnumerator Check()
    {
        while (effect.isPlaying) { yield return null; }
        Debug.Log("播放完毕");
        Object.Destroy(gameObject);
    }

}
