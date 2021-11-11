using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace App.Sound
{
    //音频集合(单独声明做一个类应该是为了ScriptableObject)
    public class SoundList : ScriptableObject
    {
        //private SoundAsset[] m_soundList;
        //public SoundAsset[] GetList { get => m_soundList; }//属性

        public SoundAsset[] SoundAssetList { get; private set; } //上面其实可以简化成这样的写法
    }

    //单个音频资源
    public class SoundAsset
    {
        string m_key;
        AudioClip m_clip;
        [Range(0f, 1f)] float m_volume = 1f;//默认最大音量

        public string Name => m_key;//名字作key
        public AudioClip Clip { get => m_clip; }
        public float Volume { get => m_volume; }
    }
}