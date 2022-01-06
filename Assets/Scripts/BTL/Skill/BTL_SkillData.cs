using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;
using BTLMode;


public class CharacterData 
{
    #region 常用枚举
    public enum Skill_ID
    {
        Skill_01,
        Skill_02,
        Skill_03
    }

    public enum TargetSide
    {
        Self,//一些自我强化技能
        Opponent//对手
    }

    public enum EventType
    {
        None,
        VFX,
        DamageType
    }

    public enum DamageType
    {
        None,
        hit,
        Fired,
        Slashed
    }

    public enum HurtSE
    {
        LightHit,
        MediumHit,
        HearyHit,
        //一些特殊音效
        Slash,//尖锐物
        Fired,//灼伤
        Paralyse//电伤
    }

    public enum SlowEffectType 
    {
        Simple,
        Curve
    }

    #endregion


    #region 
    #endregion

    //基于Spine的SkeletonData中储存的动画
    public AnimationReferenceAsset AnimAsset;

    //攻受击字段成员及其对应类
    [System.Serializable]
    public class AnimData 
    {
        #region 攻击方事件拆分

        [System.Serializable]
        public class VFXConfig//攻击时发出的特效
        {
            public GameObject prefab;//配置特效预制件
            public Transform linker;//特效生成位置(骨骼)
            public bool noLink;
            public float delay;//需要配合Timeline的地方！！！
        }
        public List<VFXConfig> vfxConfigs = new List<VFXConfig>();

        [System.Serializable]
        public class SoundConfig
        {
            public string name;//根据名字读取音效资源
            public float delay;//需要配合Timeline的地方！！！
        }
        public List<SoundConfig> soundConfigs = new List<SoundConfig>();
        #endregion


        [System.Serializable]
        public class HurtEvent//被攻击对象身上发出的特效
        {
            public string fxLinkerName;//也可以根据 敌方身上的HurtBox来动态获得位置名
            public bool fxNoLink;
            public float delay;//需要配合Timeline的地方！！！


            public EventType eventType;
            CameraShakeType cameraShakeType;
        }
        public List<HurtEvent> hurtEvents = new List<HurtEvent>();
        //也许TimeLine还需要有个Preview小窗，观看配置后打 素体模板人偶的效果？

        [System.Serializable]
        public class SlowEffect 
        {
            public SlowEffectType type;
            public float delay;//需要配合Timeline的地方！！！
            public float duration;//慢动作维持时长
            public float speedMin;//相当于全局时间的TimeScale
            public AnimationCurve curve;//暂时不知道横纵坐标分别代表什么
        }

    }



    //单一技能信息
    [System.Serializable]
    public class BTL_SkillData
    {
        public Skill_ID id;
        public TargetSide targetSide;
        public DamageType damageType;

        #region 插入动画相关字段
        public string cutinMovieName;
        public string cutinMovieSeId;
        public string cutinMovieVoiceId;
        public bool cutinMovieSkip;//调试时可跳过
        public GameObject cutinBgPrefab;//速度线，粒子(kirakira)，雾效，氛围背景等
        public Color cutinBgcolor;
        public List<AnimData> animDatas = new List<AnimData>();
        public float endWaitTime;//需要配合Timeline的地方！！！(结束后等待几秒回到普通战斗状态)
        #endregion

        public string exportFileName;
        public bool importApplyFileName;//是否用Import文件的名称用作输出文件的名称

        [System.Serializable]
        public class AnimData
        {

            public void GetPlayListData()
            {
            }

        }

        [System.Serializable]
        public class PlayList
        {

        }

    }


}


