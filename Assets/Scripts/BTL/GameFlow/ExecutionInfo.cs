using UnityEngine;

namespace BTLMode
{

	#region 一般配置Enum

	//字体
	public enum FontId
	{
		Font1,
		Font2,
		Font3,
		Font4,
		Font5,
		Font6,
		Font7,
		Font8,
		Font9,
		Font10
	}

	public enum Player
    {
        Player1,
        Player2
    }

    public enum Side
    {
        Left,
        Right
    }

    public enum Sizes
    {
        None,
        Small,
        Medium,
        High
    }

	#region 输入
	public enum ButtonPress//会替换为现有的输入判断
	{
		Foward,
		Back,
		Up,
		Down,
		Button1,
		Button2,
		Button3,
		Button4,
		Button5,
		Button6,
		Button7,
		Button8,
		Button9,
		Button10,
		Button11,
		Button12
	}

	public enum InputType
	{
		HorizontalAxis,
		VerticalAxis,
		Button
	}

	#endregion

	#endregion

	#region 格斗相关Enum（之后可以把BTL_SkillData的也整合进来）

	//---------------------------------------------------------来自moveInfo--------------------------------------
	public enum PossibleStates
	{
		Stand,
		Crouch,
		StraightJump,
		ForwardJump,
		BackJump,
		Down
	}

	public enum CombatStances
	{
		Stance1,
		Stance2,
		Stance3,
		Stance4,
		Stance5,
		Stance6,
		Stance7,
		Stance8,
		Stance9,
		Stance10
	}



	public enum DamageType//不知道为什么要这么分
	{
		Percentage,
		Points
	}
	public enum AttackType
	{
		Regular,
		Special,
		EX,
		Super
	}
	public enum ProjectileType
	{
		Shot,
		Beam
	}
	public enum HitType
	{
		HighLow,
		Low,
		Overhead,
		Launcher,
		Knockdown,
		HardKnockdown,
		Unblockable
	}
	public enum HitStrengh
	{
		Weak,
		Medium,
		Heavy,
		Crumple,
		Custom1,
		Custom2,
		Custom3
	}
	public enum HitStunType
	{
		FrameAdvantage,
		Frames
	}


	#endregion

	#region 一般配置Class

	[System.Serializable]
	public class FontOptions
	{
		public FontId fontId;
		public GameObject fontPrefab;
	}

	[System.Serializable]
    public class CameraOptions
    {
        public Vector3 initialDistance;
        public Vector3 initialRotation;
        public float initialFieldOfView;
        public float smooth = 20;
        public float minZoom = 38f;
        public float maxZoom = 54f;
    }
	#endregion


	#region 格斗相关

	//----------------------------------来自GlobalInfo-----------------------------------------
	[System.Serializable]
    public class ComboOptions
    {
        public Sizes hitStunDeterioration;
        public Sizes damageDeterioration;
        public Sizes airJuggleDeterioration;
        public float minHitStun = 1;
        public float minDamage = 5;
        public float minPushForce = 5;
        public bool neverAirRecover = false;
        public int maxCombo = 99;
    }

    [System.Serializable]
    public class BounceOptions
    {
        public Sizes bounceForce;
        public GameObject bouncePrefab;
        public float minimumBounceForce = 0;
        public float maximumBounces = 0;
        public bool bounceHitBoxes;
    }

	[System.Serializable]
	public class BlockOptions
	{
		public BlockType blockType;
		public GameObject blockPrefab;
		public float blockKillTime;
		public AudioClip blockSound;
		public bool allowAirBlock;
		public ParryType parryType;
		public float parryTiming;
		public GameObject parryPrefab;
		public float parryKillTime;
		public AudioClip parrySound;
		public Color parryColor;
		public bool allowAirParry;
		public bool highlightWhenParry;
		public Sizes blockPushForce; // NOT DONE
		public ButtonPress[] pushBlockButtons; // NOT DONE
	}

	[System.Serializable]
	public class KnockDownOptions
	{
		public float knockedOutTime = 2;
		public float getUpTime = .6f;
		public bool knockedOutHitBoxes;
		public ButtonPress[] quickStandButtons;
		public float minQuickStandTime;
		public ButtonPress[] delayedStandButtons = new ButtonPress[0];
		public float maxDelayedStandTime;
	}

	[System.Serializable]
	public class HitTypeOptions
	{
		public GameObject hitParticle;
		public float killTime;
		public AudioClip hitSound;
		public float freezingTime;
		public bool shakeCharacterOnHit = true;
		public bool shakeCameraOnHit = true;
		public float shakeDensity = .8f;
		public bool editorToggle;
	}

	[System.Serializable]
	public class HitOptions
	{
		public HitTypeOptions weakHit;
		public HitTypeOptions mediumHit;
		public HitTypeOptions heavyHit;
		public HitTypeOptions crumpleHit;
		public HitTypeOptions customHit1;
		public HitTypeOptions customHit2;
		public HitTypeOptions customHit3;
	}

	[System.Serializable]
	public class StageOptions
	{
		public string stageName;
		public Texture2D screenshot;
		public GameObject prefab;
		public AudioClip music;
		public float leftBoundary = -38;
		public float rightBoundary = 38;
	}

	[System.Serializable]
	public class GUIOptions
	{
		public FontId alertFont;
		public FontId characterNameFont;
		public FontId menuFontBig;
		public FontId menuFontSmall;
		public GUIBarOptions lifeBarOptions1;
		public GUIBarOptions gaugeBarOptions1;
		public GUIBarOptions lifeBarOptions2;
		public GUIBarOptions gaugeBarOptions2;
	}

	[System.Serializable]
	public class LanguageOptions
	{
		public string languageName = "English";
		public string start = "Start";
		public string options = "Options";
		public string credits = "Credits";
		public string selectYourCharacter = "Select Your Character";
		public string selectYourStage = "Select Your Stage";
		public string round = "Round %round%";
		public string finalRound = "Final Round";
		public string fight = "Fight!";
		public string firstHit = "First Hit!";
		public string combo = "%number% hit combo!";
		public string parry = "Parry!";
		public string victory = "%character% wins!";
		public string perfect = "Perfect!";
		public string rematch = "Rematch";
		public string quit = "Quit";
	}

	[System.Serializable]
	public class GUIBarOptions
	{
		public bool editorToggle;
		public bool previewToggle;
		public bool flip;
		public Texture2D backgroundImage;
		public Color backgroundColor;
		public Texture2D fillImage;
		public Color fillColor;
		public Rect backgroundRect;
		public Rect fillRect;
		public GameObject bgPreview;
		public GameObject fillPreview;

	}



	[System.Serializable]
	public class RoundOptions
	{
		public int totalRounds = 3;
		public float timer = 99;
		public float p1XPosition = -5;
		public float p2XPosition = 5;
		public float delayBeforeEndGame = 4;
		public AudioClip victoryMusic;
		public bool resetLifePoints = true;
		public bool resetPositions = true;
		public bool allowMovement = true;
		public bool slowMotionKO = true;
		public bool cameraZoomKO = true;
		public bool freezeCamAfterOutro = true;
	}

	//---------------------------------------------------------来自moveInfo--------------------------------------
	[System.Serializable]
	public class Projectile
	{
		public int castingFrame = 1;
		public GameObject projectilePrefab;
		public GameObject impactPrefab;
		public BodyPart bodyPart;
		public ProjectileType type;
		public Vector3 offSet;
		public int totalHits = 1;
		public bool projectileCollision;

		public bool resetPreviousHitStun = true;
		public int hitStunOnHit;
		public int hitStunOnBlock;

		public DamageType damageType;
		public float damageOnHit;
		public float damageOnBlock;
		public bool damageScaling;

		public int speed = 20;
		public int directionAngle;
		public float duration;

		public Vector2 pushForce;
		public float hitRadius;
		public HitStrengh hitStrengh;
		public HitType hitType;

		[HideInInspector] public bool damageOptionsToggle;
		[HideInInspector] public bool hitStunOptionsToggle;

		[HideInInspector]
		public bool casted;
		[HideInInspector]
		public Transform position;
	}

	

	[System.Serializable]
	public class InvincibleBodyParts
	{
		public BodyPart[] bodyParts;
		public bool completelyInvincible = true;
		public int activeFramesBegin;
		public int activeFramesEnds;
		[HideInInspector]
		public HitBox[] hitBoxes;
	}


	[System.Serializable]
	public class AppliedForce
	{
		public int castingFrame;
		public bool resetPreviousVertical;
		public bool resetPreviousHorizontal;
		public Vector2 force;
		[HideInInspector]
		public bool casted;
	}


	#endregion
}
