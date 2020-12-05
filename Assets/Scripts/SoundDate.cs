using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundDate
{
    [SerializeField] private AudioSource audio;
    [SerializeField] private AudioClip[] stepSounds;
    [SerializeField] private AudioClip[] jumpSounds;


    //暂时用不上的代码
    public void Footstep()
    {
        if (audio)
        {
            audio.PlayOneShot(stepSounds[Random.Range(0, stepSounds.Length)]);
            Debug.Log("playFootstep");
        }

    }

    public void Jump()
    {
        if (audio)
        {
            audio.PlayOneShot(jumpSounds[Random.Range(0, jumpSounds.Length)]);
            Debug.Log("playjumpSounds");
        }

    }

   public void PlayAudio()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            var audioClip = Resources.Load<AudioClip>("footsteps2");
            audio.PlayOneShot(audioClip);
        }
    }


}
