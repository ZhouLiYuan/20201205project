using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static void PlaySound(string name)
    {
        new GameObject().AddComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>)
            //播放完销毁
    }
} 
