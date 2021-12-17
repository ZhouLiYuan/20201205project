using Role;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;



namespace Role.SpineRole
{
    //Spine_Animation版本  
    public class SpineRole : RoleEntity
    {
        //需要暴露在odin的变量
        public float playBackSpeed = 1f;//1为正常速度

        private FSM generalFsm;

        Atlas atlas;

        SkeletonAnimation skeletonAnimation;
        SkeletonJson json;

        Skeleton skeleton;
        SkeletonData skeletonData;

        Spine.AnimationState state;
        AnimationStateData stateData;

        AnimationReferenceAsset idle01, move01, attack01, damage01;//对应的格式后缀名是.anim?

        //或者作为一种快速打case的手段
        public enum RoleState
        {
            //Idle,
            //Move,
            //Damaged,
            //Attack
            idleAnim,
            moveAnim,
            jumpAnim,
            attackAnim
        }
        public RoleState roleState;


        #region 动画名称
        private const string idleAnim = "Idle01";
        private const string moveAnim = "Move01";
        private const string jumpAnim = "Jump01";
        private const string attackAnim = "Attack01";
        private string currentAnim;
        #endregion

        [SpineEvent] public string footstepEventName = "footstep";

        TrackEntry trackEntry;//类似分层动画（或者类似Unity的Animator？//不要在dispose侦听器事件发生后保留该引用。


        //常用骨骼Transform(放置特效用)
        public Transform root, foot_R, foot_L, hand_R, hand_L;

        public override void Init(GameObject obj)
        {
            skeleton = skeletonAnimation.skeleton;//该用skeleton字段 还是Skeleton属性？
            
            //第一层级
            base.Init(obj);

            //Load模块
            atlas = ResourcesLoader.LoadAtlas();
            AtlasAttachmentLoader attachmentLoader = new AtlasAttachmentLoader(atlas);//atlasAttachmentLoader负责atlas文件的解析
            json = new SkeletonJson(attachmentLoader);
            //skeletonData = json.ReadSkeletonData("mySkeleton.json");//这个读取方法应该用绝对路径吗？
            //skeletonData = skeleton.Data;
            skeletonData = ResourcesLoader.LoadSkeletonData();

            skeletonAnimation = Transform.GetComponent<SkeletonAnimation>();
            if (skeletonAnimation == null) return;
            Debug.Log(skeletonAnimation.name);//获取角色名


            state = skeletonAnimation.state;
            stateData = new AnimationStateData(this.skeletonData);
            stateData.SetMix(idleAnim, moveAnim, 0.2f);
            stateData.SetMix(moveAnim, attackAnim, 0.4f);

            skeletonAnimation.AnimationState.TimeScale = playBackSpeed;//统一设置所有动画的播放速度？

            trackEntry = state.SetAnimation(0, moveAnim, true);
            trackEntry.MixDuration = 0.6f;//统一设置所有动画间过渡的混合

            //或者能不能直接从_bone获得
            root = Transform.Find("SkeletonUtility-SkeletonRoot/root");
            hand_R = Transform.Find("SkeletonUtility-SkeletonRoot/root/body/body2/arm_R1/arm_R2/hand_R");

       
           
            //可以在SkeletonData查看,目前命名bone slot attachment都是一致的
            


            //换装函数
            skeleton.SetAttachment("body", "body");
            skeleton.GetAttachment("body", "body");


            #region spine的事件（类似InputSystem）
            state.Start += State_Start;//当动画开始播放时触发。
            state.End += State_End;//当动画被清除(或中断)时触发
            state.Interrupt += State_Interrupt;//当新的动画被设置并且当前有一个动画还在播放时触发
            state.Complete += State_Complete;//当动画完成时触发
            state.Event += State_Event;//动画事件触发

            //lambda表达式：
            state.Complete += (TrackEntry trackEntry) =>
            {
                Debug.Log("");
            };
            #endregion

            #region 貌似是过时的API（都是访问级别报错）原因大概率是从字段编程了属性(后备字段自然无法访问) ，注释掉的部分是报错原因不明的
            skeleton.R = 0.5f;//可以用来做一些中毒变色的效果？

            //获取骨骼信息
            Bone _bone = skeleton.FindBone("body");
            Slot _slot = skeleton.FindSlot("body");
            Debug.Log(_bone.Parent.ToString());//骨骼的父骨骼
            Debug.Log(_bone.Data.Length.ToString());//骨骼长度
            Debug.Log(_bone.Rotation.ToString());//骨骼旋转
            Debug.Log(_bone.Data.ScaleX.ToString());//骨骼放缩
            Debug.Log(_bone.Data.ScaleY.ToString());
            Debug.Log(_bone.Data.ShearX.ToString());// 获取骨骼倾斜
            Debug.Log(_bone.Data.TransformMode.ToString());
            //Debug.Log(_bone.data.inheritRotation.ToString());// 是否旋转值相对父骨骼 true是相对父骨骼
            //Debug.Log(_bone.data.inheritScale.ToString());//是否放缩值相对父骨骼 true是相对父骨骼

            //获取插槽信息
            Debug.Log(_slot.Data.Name);//获取slot的名字
            Debug.Log(_slot.Data.BoneData.Name);//获取slot挂靠的bone
            Debug.Log(_slot.R.ToString());//获取slot的颜色R值
            Debug.Log(_slot.Data.AttachmentName);//获取pose下slot的attachment名
            //_slot.Data.AdditiveBlending = true;//获取或设置slot时候用additive blending来渲染  

            skeleton.FindSlot("torso");//根据slot名获取slot
            skeleton.FindBone("root");//根据骨骼名获取骨骼
            //.Attachment.Name


            //Debug.Log(skeleton.bones[5].ToString()); //获取所有骨骼数组list<spine.Bone>
            //Debug.Log(skeleton.slots[5].ToString());//获取所有插槽数组
            //Debug.Log(skeleton.Data.animations[0].name.ToString());//获取所有动画数组
            //Debug.Log(skeleton.FindBoneIndex("root").ToString());//根据骨骼名获取骨骼的index
            //Debug.Log(skeleton.FindSlotIndex("torso").ToString());//根据slot名获取slot index
            //Debug.Log(skeleton.Data.FindAnimation("walk").Name);//根据动画名获取动画 

            ////skin(可以用哥布林做实验)
            //Debug.Log(skeletonAnimation.skeleton.Data.skins[0].name.ToString());//获取所有 skin
            //Spine.Attachment _attachment = skeletonAnimation.skeleton.Data.skins[0].GetAttachment(5, "left lower leg");//从skin通过slot的index attachment的名获取attachment


            ////Animation
            //Debug.Log(skeletonAnimation.skeleton.data.animations[0].duration.ToString());//动画持续时间？
            //List<Spine.Timeline> _timeline = skeletonAnimation.skeleton.data.animations[0].timelines;//获取动画timeline


            //Skeleton
            //Debug.Log(skeletonData.bones[0].ToString()); //获取所有骨骼
            //Debug.Log(skeletonData.slots[0].ToString());//获取所有slots
            //Debug.Log(skeletonAnimation.skeleton.drawOrder[0].attachment.Name);//获取所有DrawOder
            //Debug.Log(skeletonAnimation.skeleton.Skin.Name);//获取当前skin名
            //skeletonAnimation.skeleton.R = 0.5f;//对skin的整体偏色 有rgba四个参数
            //Debug.Log(skeleton.Time.ToString());//?
            //skeletonAnimation.skeleton.flipX = true;//skeleton翻转轴向
            //skeletonAnimation.skeleton.flipY = true;
            skeletonAnimation.skeleton.X = 0;//skeleton的坐标
            skeletonAnimation.skeleton.Y = 0;

            //获取spine部件的位置坐标
            Vector3 pos = skeletonAnimation.skeleton.FindSlot("head").Bone.GetWorldPosition(Transform);
            Debug.Log("head坐标：" + pos);
            #endregion

            generalFsm = new GeneralFSM();
            generalFsm.AddState<IdleState>().SetRole(this);
        }

        #region 事件对应的回调方法

        //配合各状态enum + case似乎可以实现类似StateMachineBehaviour的OnStateEnter/Exit效果？
        //自己再实现个Update方法好像就可以组成一个状态机了 但这会让每个方法都非常臃肿

        private void State_Start(TrackEntry trackEntry)//OnStateEnter
        {

        }

        private void State_Complete(TrackEntry trackEntry)//On one duration finsh
        {
            //trackEntry.Animation.Name
            //switch (roleState)
            //{
            //    case RoleState.Idle:
            //        break;
            //    case RoleState.Move:
            //        break;
            //    case RoleState.Damaged:
            //        break;
            //    case RoleState.Attack:
            //        break;
            //    default:
            //        break;
            //}
        }

        private void TrackEntry_Complete(TrackEntry trackEntry) { }

        private void State_End(TrackEntry trackEntry)//OnStateExit
        {
            //也可以不用枚举
            switch (trackEntry.Animation.Name)
            {
                case idleAnim:
                    break;
                case moveAnim:
                    break;
                case jumpAnim:
                    break;
                case attackAnim:
                    break;
                default:
                    break;
            }
        }

        private void State_Interrupt(TrackEntry trackEntry)//播放混合动画时调用
        {
            skeletonAnimation.skeleton.SetToSetupPose();
            state.ClearTracks();
        }


        private void State_Event(TrackEntry trackEntry, Spine.Event e)
        {
            // 如果事件名为footstep就播放声音
            if (e.Data.Name == footstepEventName)
            {
                Debug.Log("Play a footstep sound!");
            }
        }

        #endregion

        #region 设置Spine数据方法：动画，附件，重置
        public void ResetSpineData()
        {
            //设置初始动作
            skeleton.SetBonesToSetupPose();
            skeleton.SetSlotsToSetupPose();
            skeleton.SetToSetupPose();

            state.ClearTracks();
        }

        //同一个spine对象是可以同时播放多个动画的（trackIndex类似于Animator的LayerIndex？)
        //播放动画
        private void SetAnimation(AnimationReferenceAsset anim, bool loop, float timeScale)
        {
            if (anim.name.Equals(currentAnim)) return; //如果已经在播放就不必重新播放
            var trackEntry = state.SetAnimation(0, anim, loop);
            trackEntry.TimeScale = timeScale;//暂时默认动画不分层
            trackEntry.Complete += TrackEntry_Complete;

            currentAnim = anim.name;

            //其他动画API
            //skeletonAnimation.state.SetAnimation(0, "Attack01", false);
            //skeletonAnimation.state.AddAnimation(0, "Attack01", true, 0);//安排该动画在此track当前动画或最后排队的动画后播放
        }

        //以交互形式实现某些附件的显示与非显示
        //显示或非显示slot（适用于slot下只有一个Attachment，且两者同名时？）
        void ToggleAttachment(string slotName)
        {
            if (skeletonAnimation.skeleton.FindSlot(slotName).Attachment == null)
            {
                string attachmentName = slotName;
                skeleton.SetAttachment(slotName, attachmentName); //可实现SheetAnimation功能
            }
            else
            {
                skeleton.SetAttachment(slotName, null);
            }
        }
        #endregion

        #region State相关方法
        public override void TurnFace()
        {
            //由于 PlayerRole 和 Enemy 是两套不同反转机制，需要分两套写
            if (IsFaceRight) return;
            else { skeletonAnimation.skeleton.FlipX = true; }
        }

        void Idle()
        {
            if (skeletonAnimation.AnimationName != "Idle01")
                SetAnimation(idle01, true, 1f);
        }

        void Move()
        {
            if (skeletonAnimation.AnimationName != "Move01")
                SetAnimation(move01, true, 1f);
        }

        public void GetDamage()
        {
            if (skeletonAnimation.AnimationName != "Damage01")
                SetAnimation(damage01, false, 1f);
        }

        public void Attack()
        {
            if (skeletonAnimation.AnimationName != "Attack01")
                SetAnimation(attack01, false, 1f);
        }

        #endregion

        #region 生命周期 
        void OnUpdate()
        {
            //这两个API一般配套
            state.Update(0);// 更新内部状态
            state.Apply(skeleton);//不会更改任何内部状态，使一个AnimationState可应用到多个骨架 apply调用是下一帧的

            skeletonAnimation.skeleton.Update(0.5f);
        }


        void OnDestroy()
        {
            skeletonAnimation.state.Start -= State_Start;//诸如此类解耦操作
        }
        #endregion
    }




    //Spine_Mecanim版本
    public class SpineCharacter : RoleEntity
    {
        SkeletonMecanim mecanim;

        public override void Init(GameObject obj)
        {
            //    //第一层级
            //    base.Init(obj);
            mecanim = Transform.GetComponent<SkeletonMecanim>();
            //    //角色之间的绘制排序
            //    renderer.sortingOrder = -5;
        }
    }
}