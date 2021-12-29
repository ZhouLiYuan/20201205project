using UnityEngine;
using Spine;
using Spine.Unity;

//不用原本的FSM系统(前提条件，状态和动画一一对应时)
namespace Role.SpineRole
{
    //状态机其实思路和之前版本的差不多，可以参考例子4 Object Oriented Sample
    //非Spine版本PlayerRoleState是用State切换动画播放，Unity原生动画系统则是状态和动画一一对应
    //Spine版本也同样，只是多了一些动画事件可以用（动画事件和State还是有很多用法的不同的）
    public class SpineRoleState : State
    {
        public PlayerRole_Spine Role { get; private set; }
        protected GameObject role_Gobj => Role.GameObject;

        public Spine.AnimationState state => Role.state;//当Animator用？

        SkeletonAnimation skeletonAnimation => Role.skeletonAnimation;
        Skeleton skeleton => Role.skeleton;
        //SkeletonData skeletonData => Role.skeletonData;

        //一些输入
        protected Vector2 InputAxis => Role.inputAxis;
        protected float MoveSpeed => Role.moveSpeed;
        protected float JumpSpeed => Role.jumpSpeed;

        protected Vector2 Velocity
        {
            get { return Role.Velocity; }
            set { Role.Velocity = value; }
        }

        protected string animName;
        protected Spine.Animation nextAnimation;
     

        TrackEntry trackEntry;//不要在dispose侦听器事件发生后保留该引用。

        #region 事件相关字段
        // you can cache event data to save the string comparison
        Spine.EventData targetEventData;
        string targetEventName = "targetEvent";
        //string targetEventNameInFolder = "eventFolderName/targetEvent";
        [SpineEvent] public string footstepEventName = "footstep";
        #endregion

        public void SetPlayerRole(PlayerRole_Spine role)
        {
            Role = role;
            Init();
        }

        private void Init()
        {
            //会自动获得当前 实现类而不是基类的实例
            var fullName = GetType().Name;
            animName = fullName.Substring(0, fullName.LastIndexOf("S"))+"01";
        }

        #region 事件对应回调方法

        //配合各状态enum + case似乎可以实现类似StateMachineBehaviour的OnStateEnter/Exit效果？
        //自己再实现个Update方法好像就可以组成一个状态机了 但这会让每个方法都非常臃肿

        protected virtual void State_Start(TrackEntry trackEntry)//调用时机：OnStateEnter后一帧？
        {
        }

        protected virtual void State_Complete(TrackEntry trackEntry)//On one duration finsh
        {
        }

        protected virtual void State_End(TrackEntry trackEntry)//OnStateExit前一帧？
        {
        }

        protected virtual void OnSpineAnimationDispose(TrackEntry trackEntry)
        {
            // Add your implementation code here to react to dispose events
        }

        protected virtual void State_Interrupt(TrackEntry trackEntry)//播放混合动画时调用
        {
            skeletonAnimation.skeleton.SetToSetupPose();
            state.ClearTracks();
        }

        protected virtual void State_Event(TrackEntry trackEntry, Spine.Event e)
        {
            // 如果事件名为footstep就播放声音
            if (e.Data.Name == footstepEventName)
            {
                Debug.Log("Play a footstep sound!");
            }
        }

        protected virtual void OnUserDefinedEvent(TrackEntry trackEntry, Spine.Event e)
        {
            if (e.Data == targetEventData)
            {
                // Add your implementation code here to react to user defined event
            }
        }
        #endregion

        #region 动画API
        //同一个spine对象是可以同时播放多个动画的（trackIndex类似于Animator的LayerIndex？)
        //播放动画
        protected void SetAnimation(Spine.Animation anim, int trackIndex = 0, bool loop = false, float mixDuration = 0f, float timeScale = 1f)
        {
            if (anim.Name.Equals(animName)) return; //如果已经在播放就不必重新播放
            var trackEntry = state.SetAnimation(trackIndex, anim, loop);
            trackEntry.MixDuration = mixDuration;
            trackEntry.TimeScale = timeScale;//暂时默认动画不分层
            //trackEntry.Complete += TrackEntry_Complete;
            animName = anim.Name;
        }

        //重载：通过动画名称在自身SkeletonData中寻找对应动画(变相实现了相对路径的效果)
        protected void SetAnimation(int trackIndex = 0, string animationName = "Idle01", bool loop = false, float mixDuration = 0f, float timeScale = 1f)
        {
            if (animationName.Equals(animName)) return; //如果已经在播放就不必重新播放
            var trackEntry = state.SetAnimation(trackIndex, animationName, loop);
            trackEntry.MixDuration = mixDuration;
            trackEntry.TimeScale = timeScale;//暂时默认动画不分层
            animName = animationName;
        }

        //Add是添加播放任务，Set是直接打断，然后播放希望Set的动画内容
        protected void AddAnimation(int trackIndex = 0, string animationName = "Idle01", bool loop = false, float delay = 0f, float timeScale = 1f)
        {
            var trackEntry = state.AddAnimation(trackIndex, animationName, loop, 0);//安排该动画在此track当前动画或最后排队的动画后播放
            trackEntry.TimeScale = timeScale;//暂时默认动画不分层
        }
        #endregion

        #region 换皮(Attachment）
        //以交互形式实现某些附件的显示与非显示
        //显示或非显示slot（适用于slot下只有一个Attachment，且两者同名时？）
        protected void ToggleAttachment(string slotName)
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

        public override void OnEnter()
        {
            #region state的事件（类似InputSystem）
            targetEventData = skeletonAnimation.Skeleton.Data.FindEvent(targetEventName);

            state.Start += State_Start;//当动画开始播放时触发。
            state.End += State_End;//当动画被清除(或中断)时触发
            state.Interrupt += State_Interrupt;//当新的动画被设置并且当前有一个动画还在播放时触发
            state.Complete += State_Complete;//当动画完成时触发
            state.Event += State_Event;//动画事件触发

            //Spine.TrackEntry trackEntry = state.SetAnimation(0, "walk", true);
            //trackEntry.Dispose += OnSpineAnimationDispose;

            //lambda表达式：
            state.Complete += (TrackEntry trackEntry) =>
            {
                Debug.Log("");
            };
            #endregion

            //播放状态对应的动画
            //SetAnimation(0, animName);
            Debug.Log($"正在播放Spine动画{animName}");
        }

        public override void OnExit() 
        {
            state.Start -= State_Start;
            state.End -= State_End;
            state.Interrupt -= State_Interrupt;
            state.Complete -= State_Complete;
            state.Event -= State_Event;
        }

        public override void OnUpdate(float deltaTime)
        {

        }
    }
}


