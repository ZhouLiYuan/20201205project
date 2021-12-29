using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace App.Sound 
{
    public class SoundManager : SingletonMonoBehaviour<SoundManager>
    {
        [Title("声音管理器")]
        //BGM
        [ShowInInspector]
        [TabGroup("BGM")]//用于分类
        private SoundList m_bgmList;
        //SE
        //[SceneObjectsOnly]
        //[Required]
        [TabGroup("SE"), ShowInInspector]
        private GameObject m_sePrefab;
        [TabGroup("SE"), ShowInInspector]
        private SoundList m_seList;


        [TabGroup("BGM"), ShowInInspector] 
        Dictionary<string, SoundAsset> BgmNameDic { get; set; }
        [TabGroup("SE"), ShowInInspector] 
        Dictionary<string, SoundAsset> SeNameDic { get; set; }

        [Space]
        [ShowInInspector]
        [LabelText("Music Clip")]
        AudioSource m_source;
        AudioSource CachedSource//缓存(可以理解为currentAudioSource)
        {
            get 
            {
                if (m_source == null) 
                {
                    m_source = gameObject.AddComponent<AudioSource>();
                    //m_source = GetComponent<AudioSource>();
                }
                return m_source;
            }
        }

        [ShowInInspector]
        public float CurrentBgmTime => CachedSource.time;

        //功能实验代码

        //[ShowInInspector]
        [TabGroup("BGM")]
        [InlineButton("PauseBgm")]//第一个""里是成员方法名
        [InlineButton("StopBgm", "Stop")]//后面是重命名在面板中按钮名称
        [InlineButton("PlayBgm")]//该方法(有且有一个参数)特性时，修饰的字段类型必须和方法参数类型相同(否则会报错)
        public string clipName => CachedSource.clip.name;

        [Space]
        [ShowInInspector]
        //[Required]//强制要求非空(否则报错)

        [TabGroup("Music", "BGM")]//一个成员可以同时属于两个TabGroup分组(这好像是旧版的功能当前版本不一样)
        [InlineEditor(InlineEditorModes.SmallPreview)]//可在Inspector中播放音频
        AudioClip currentAudioClip;

        [ShowInInspector]
        [TabGroup("Music")]
        [ValueDropdown("musicList")]//这个特性好像必须和List类型配合
        [InlineEditor(InlineEditorModes.SmallPreview)]
        List<AudioClip> musicList;////可在Inspector中拖拽加入音频


        protected override void Awake()
        {
            base.Awake();
            DontDestroy();
            Init();
        }

        private void Init() 
        {
            BgmNameDic = new Dictionary<string, SoundAsset>();
            SeNameDic = new Dictionary<string, SoundAsset>();

            //将数组中的元素放进Dic中
            foreach (var bgm in m_bgmList.SoundAssetList)
            {
                SeNameDic.Add(bgm.Name, bgm);
            }

            foreach (var se in m_seList.SoundAssetList) 
            {
                SeNameDic.Add(se.Name, se);
            }

            if(m_sePrefab == null) m_sePrefab = ResourcesLoader.LoadSEPrefab();
        }

        //BGM
        public void PlayBgm(string key,float time =0f) 
        {
            if (BgmNameDic.TryGetValue(key,out var asset)) 
            {
                if (asset.Clip == CachedSource.clip) //如果查找到的clip 和 当前Inspector设置的一样，就不做操作
                {
                    //CachedSource.Play();
                    return;
                }

                CachedSource.clip = asset.Clip;
                CachedSource.volume = asset.Volume;
                CachedSource.time = time;//播放进度
                CachedSource.Play();//只能播放一种音效

                ////只播放一次，可同时播放多个音效API
                //CachedSource.PlayOneShot(asset.Clip);//2D音效(实例方法)
                //AudioSource.PlayClipAtPoint(asset.Clip, Vector3.zero);//3D音效(静态方法)
            }
        }

        public void PauseBgm() 
        {
            CachedSource.Pause();
        }

        public void StopBgm() 
        {
            CachedSource.Stop();
            CachedSource.clip = null;//终止播放Bgm
        }

        //SE
        public AudioSource PlaySE(string key)
        {
            if (SeNameDic.TryGetValue(key, out var asset))
            {
                var source = createSEObj(asset);
                source.Play();
                Destroy(source.gameObject, source.clip.length);//在播放完后销毁source所在Gobj
                return source;//Gobj销毁后组件还存在吗？
            }
            return null;
        }

        //根据asset动态配置 AudioSource组件
        private AudioSource createSEObj(SoundAsset asset) 
        {
            var obj = Instantiate(m_sePrefab);
            obj.name = $"SE_{asset.Name}";

            var source = obj.GetComponent<AudioSource>();
            source.clip = asset.Clip;
            source.volume = asset.Volume;
            return source;
        }
    }

}

////一些API
//[SerializeField] private AudioSource audio;
//[SerializeField] private AudioClip[] stepSounds;
//[SerializeField] private AudioClip[] jumpSounds;
//void Footstep()
//{
//    if (audio)
//    {
//        audio.PlayOneShot(stepSounds[Random.Range(0, stepSounds.Length)]);
//        Debug.Log("playFootstep");
//    }
//}

//void Jump()
//{
//    if (audio)
//    {
//        audio.PlayOneShot(jumpSounds[Random.Range(0, jumpSounds.Length)]);
//        Debug.Log("playjumpSounds");
//    }
//}