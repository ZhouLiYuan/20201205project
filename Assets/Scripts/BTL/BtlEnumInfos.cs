//战斗中常用的枚举
namespace BTLMode
{
	public enum BodyPart
	{
		none,
		head,
		upperTorso,
		lowerTorso,
		leftUpperArm,
		rightUpperArm,
		leftForearm,
		rightForearm,
		leftHand,
		rightHand,
		leftThigh,
		rightThigh,
		leftCalf,
		rightCalf,
		leftFoot,
		rightFoot,
		custom1,
		custom2,
		custom3,
		custom4,
		custom5,
		custom6,
		custom7,
		custom8,
		custom9
	}

	//暂时不知道有什么用
	public enum BlockType
	{
		None,
		HoldBack,
		AutoBlock,
		HoldButton1,
		HoldButton2,
		HoldButton3,
		HoldButton4,
		HoldButton5,
		HoldButton6,
		HoldButton7,
		HoldButton8,
		HoldButton9,
		HoldButton10,
		HoldButton11,
		HoldButton12
	}


	public enum CollisionType
	{
		bodyCollider,
		hitCollider,
		noCollider
	}

	public enum HitBoxType
	{
		high,
		mid,
		low
	}

	public enum ParryType//回避类型
	{
		None,
		TapBack,
		TapForward,
		TapButton1,
		TapButton2,
		TapButton3,
		TapButton4,
		TapButton5,
		TapButton6,
		TapButton7,
		TapButton8,
		TapButton9,
		TapButton10,
		TapButton11,
		TapButton12
	}
}