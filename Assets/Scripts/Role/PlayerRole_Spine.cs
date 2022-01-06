using Role;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;
using static Spine.Skin;
using Role.SelectableRole;


namespace Role.SpineRole
{
    //Spine_Animation版本(暂时用于BTL模式)
    public class PlayerRole_Spine : ControllableRole<BtlPlayerInput>
    {
        #region 输入trigger
        public bool IsLightPunchPressed { get; protected set; }
        public bool IsMediumPunchPressed { get; protected set; }
        #endregion
        public static List<Collider2D> p1_HitColliders;

        private FSM generalFsm;
        private FSM subFsm;


        //-------------------------------Spine相关--------------------------------

        //需要暴露在odin的变量
        public float playBackSpeed = 1f;//1为正常速度

        //充当Animator的工作?
        public Spine.AnimationState state;
        public AnimationStateData stateData;

        public AnimationReferenceAsset idle01, move01, attack01, damage01;
        //idle01 = ResourcesLoader.LoadAnim_Spine();
        #region 动画名称
        private const string idleAnim = "Idle01";
        private const string moveAnim = "Move01";
        private const string jumpAnim = "Jump01";
        private const string attackAnim = "Attack01";
        #endregion

        public SkeletonAnimation skeletonAnimation;
        public Skeleton skeleton;
        public SkeletonData skeletonData;

        public SpineRoleState currentState;
        public string currentAnimName;

        //常用骨骼Transform(放置特效用)
        public Transform root, foot_R, foot_L, hand_R, hand_L;

        #region 初始化

        //---------------------------------------<初始化>--------------------------------------------------

        public override void Init(GameObject obj)
        {
            //暂时的一些配置信息
            maxHP = 100;
            HP = 100;



            //第一层级
            base.Init(obj);
            skeletonAnimation = Transform.GetComponent<SkeletonAnimation>();
            skeleton = skeletonAnimation.Skeleton;
            skeletonData = skeleton.Data;

            //trackEntry = state.SetAnimation(0, moveAnim, true);
            //trackEntry.MixDuration = 0.6f;//统一设置所有动画间过渡的混合

            state = skeletonAnimation.AnimationState;
            //state = skeletonAnimation.state;
            stateData = new AnimationStateData(this.skeletonData);
            stateData.SetMix(idleAnim, moveAnim, 0.2f);
            stateData.SetMix(moveAnim, attackAnim, 0.4f);

            idle01 = ResourcesLoader.LoadAnim_Spine(); 
            move01 = ResourcesLoader.LoadAnim_Spine("JokerKung", moveAnim); 
            attack01 = ResourcesLoader.LoadAnim_Spine("JokerKung", attackAnim);

            //可以在SkeletonData查看,目前命名bone slot attachment都是一致的
            #region 获取骨骼信息
            //或者能不能直接从_bone获得
            root = Transform.Find("SkeletonUtility-SkeletonRoot/root");
            hand_R = Transform.Find("SkeletonUtility-SkeletonRoot/root/body/body2/arm_R1/arm_R2/hand_R");

            Bone _bone = skeleton.FindBone("body");
            Debug.Log(_bone.Parent.ToString());//骨骼的父骨骼
            Debug.Log(_bone.Data.Length.ToString());//骨骼长度
            Debug.Log(_bone.Rotation.ToString());//骨骼旋转
            Debug.Log(_bone.Data.ScaleX.ToString());//骨骼放缩
            Debug.Log(_bone.Data.ScaleY.ToString());
            Debug.Log(_bone.Data.ShearX.ToString());// 获取骨骼倾斜
            Debug.Log(_bone.Data.TransformMode.ToString());
            //Debug.Log(_bone.data.inheritRotation.ToString());// 是否旋转值相对父骨骼 true是相对父骨骼
            //Debug.Log(_bone.data.inheritScale.ToString());//是否放缩值相对父骨骼 true是相对父骨骼
            #endregion


            //换装函数
            skeleton.SetAttachment("body", "body");
            skeleton.GetAttachment("body", "body");


            #region 貌似是过时的API（都是访问级别报错）原因大概率是从字段编程了属性(后备字段自然无法访问) ，注释掉的部分是报错原因不明的
          
            //skeleton.R = 0.5f;//骨骼变色，可以用来做一些中毒变色的效果？

            //获取插槽信息
            var slots = Transform.GetComponent<SkeletonAnimation>().Skeleton.Slots;//根据这样就可以根据元素获得Slot的Index了(自己写一个Foreach的扩展方法)
            Slot _slot = skeleton.FindSlot("body");

            Debug.Log(_slot.Data.Name);//获取slot的名字
            Debug.Log(_slot.Data.BoneData.Name);//获取slot挂靠的bone
            Debug.Log(_slot.R.ToString());//获取slot的颜色R值
            Debug.Log(_slot.Data.AttachmentName);//获取pose下slot的attachment名
            //_slot.Data.AdditiveBlending = true;//获取或设置slot时候用additive blending来渲染  

            skeleton.FindSlot("body");//根据slot名获取slot
            skeleton.FindBone("body");//根据骨骼名获取骨骼
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

            //generalFsm = new GeneralFSM();
            //generalFsm.AddState<IdleState>().SetRole(this);
        }

        protected override void InitFSM()
        { 
            base.InitFSM();
            //每个FSM创建后记得关联OnUpdate()和OnFixUpdate()；

            //分不同的FSM其实就是分层处理
            generalFsm = new Spine_GeneralFSM();

            generalFsm.AddState<IdleState>().SetPlayerRole(this);
            generalFsm.AddState<MoveState>().SetPlayerRole(this);
            generalFsm.AddState<JumpState>().SetPlayerRole(this);

            generalFsm.AddState<DamagedState>().SetPlayerRole(this);

            subFsm = new Spine_AttackFSM();
            subFsm.AddState<PreSubActionState>().SetPlayerRole(this);

            subFsm.AddState<LightPunchState>().SetPlayerRole(this);
            subFsm.AddState<MediumPunchState>().SetPlayerRole(this);
        }

        public override void BindInput(BtlPlayerInput inputHandler)
        {
            base.BindInput(inputHandler);

            playerInput.LightPunch.performed += context => IsLightPunchPressed = true;
            playerInput.LightPunch.canceled += context => IsLightPunchPressed = false;

            playerInput.MediumPunch.performed += context => IsMediumPunchPressed = true;
            playerInput.MediumPunch.canceled += context => IsMediumPunchPressed = false;
        }

        public void ResetSpineData()
        {
            //设置初始动作
            //skeleton.SetBonesToSetupPose();// 将所有bone、constraint重置到setup状态
            //skeleton.SetSlotsToSetupPose();// 将所有slot重置到setup状态
            skeleton.SetToSetupPose();// 将所有bone和slot重置到setup状态(上面方法的二合一)
            // 移除所有轨道上的动画，但保持skeleton为当前pose
            // 务必在SetToSetupPose之后调用该函数，因为轨道上仍有动画
            state.ClearTracks();
        }

        #endregion

        #region State相关方法
        public override void TurnFace()
        {
            ////由于 PlayerRole 和 Enemy 是两套不同反转机制，需要分两套写
            //if (IsFaceRight)
            //{
            //    skeleton.ScaleX = 1;
            //    return;
            //}
            //else { skeleton.ScaleX = -1; }

            if ((skeleton.ScaleX > 0) != IsFaceRight)
                skeleton.ScaleX = IsFaceRight ? 1f : -1f;
            //skeleton.FlipX = true; 过时API
        }




        //void Idle()
        //{
        //    if (skeletonAnimation.AnimationName != "Idle01")
        //        SetAnimation(idle01, true, 1f);
        //}

        //void Move()
        //{
        //    if (skeletonAnimation.AnimationName != "Move01")
        //        SetAnimation(move01, true, 1f);
        //}

        //public void GetDamage()
        //{
        //    if (skeletonAnimation.AnimationName != "Damage01")
        //        SetAnimation(damage01, false, 1f);
        //}

        //public void Attack()
        //{
        //    if (skeletonAnimation.AnimationName != "Attack01")
        //        SetAnimation(attack01, false, 1f);
        //}

        #endregion

        #region 生命周期 
        protected override void OnUpdate(float deltaTime)
        {
     
            generalFsm.Update(deltaTime);
            subFsm.Update(deltaTime);

            ////currentState.OnUpdate(deltaTime);
            //skeletonAnimation.skeleton.Update(0.5f);
            //skeletonAnimation.AnimationState.TimeScale = playBackSpeed;//统一设置所有动画的播放速度？

            ////这两个API一般配套
            //state.Update(0);// 更新内部状态
            //state.Apply(skeleton);//不会更改任何内部状态，使一个AnimationState可应用到多个骨架 apply调用是下一帧的

            //// 添加受击框和打击框(也许应该动态设置)
            //p1_HitColliders.AddRange(GameObject.GetComponents<Collider2D>());
            //p1_HitColliders.AddRange(GameObject.GetComponentsInChildren<Collider2D>());
        }


        void OnDestroy()
        {
            //skeletonAnimation.state.Start -= State_Start;//诸如此类解耦操作
        }
        #endregion
    }
}




//    //Spine_Mecanim版本
//    public class SpineCharacter : RoleEntity
//    {
//        SkeletonMecanim mecanim;

//        public override void Init(GameObject obj)
//        {
//            //    //第一层级
//            //    base.Init(obj);
//            mecanim = Transform.GetComponent<SkeletonMecanim>();
//            //    //角色之间的绘制排序
//            //    renderer.sortingOrder = -5;
//        }
//    }
//}


//Spine Example中有意思的设计

//public void TryJump()
//{
//    StartCoroutine(JumpRoutine());
//}

//IEnumerator JumpRoutine()
//{
//    //if (state == SpineBeginnerBodyState.Jumping) yield break;//相当于Return

//    //state = SpineBeginnerBodyState.Jumping;

//    // Fake jumping.
//    {
//        var pos = Transform.localPosition;
//        const float jumpTime = 1.2f;
//        const float half = jumpTime * 0.5f;
//        const float jumpPower = 20f;
//        for (float t = 0; t < half; t += Time.deltaTime)
//        {
//            float d = jumpPower * (half - t);
//            Transform.Translate((d * Time.deltaTime) * Vector3.up);
//            yield return null;
//        }
//        for (float t = 0; t < half; t += Time.deltaTime)
//        {
//            float d = jumpPower * t;
//            Transform.Translate((d * Time.deltaTime) * Vector3.down);
//            yield return null;
//        }
//        Transform.localPosition = pos;
//    }

//    //state = SpineBeginnerBodyState.Idle;
//}