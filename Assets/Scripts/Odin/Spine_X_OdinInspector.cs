using Role.SpineRole;
using Sirenix.OdinInspector;
using Spine.Unity;
using System;

[Serializable, TypeInfoBox("运行时数值调试器")]
public class Spine_X_OdinInspector : SerializedMonoBehaviour
{
	#region SpineAPI
	// [SpineAnimation] attribute allows an Inspector dropdown of Spine animation names coming form SkeletonAnimation.
	[SpineAnimation]
	public string runAnimationName;

	[SpineEvent] public string footstepEventName = "footstep";

	[SpineSlot]
	public string eyesSlot;

	[SpineAttachment(currentSkinOnly: true, slotField: "eyesSlot")]
	public string eyesOpenAttachment;

	#endregion

	#region odinAPI
	[TabGroup("Player")]
	[GUIColor(1, .5f, 0, 1)]//橙色
	[ShowInInspector, Searchable]
	[PropertyOrder(0)]//属性顺序
	[InlineButton("SynchronizePlayerRole", "运行时同步设置")]
	private PlayerRole_Spine playerRole;//只会在InSpector中序列化引用中的public字段（属性无法序列化）
	private void SynchronizePlayerRole() { playerRole = BtlCharacterManager.p1_Role; }


	[TabGroup("Player")]
	[ShowInInspector, EnableGUI]
	[PropertyOrder(1)]
	[InlineButton("SetPlayBackSpeed", "运行时设置")]
	public float playBackSpeed;
	private void SetPlayBackSpeed() { playerRole.playBackSpeed = playBackSpeed; }
	#endregion
}
