using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BTLMode
{
	[System.Serializable]
	public class HitBox
	{
		public BodyPart bodyPart;
		public HitBoxType type;
		public Transform position;
		public float radius = .5f;
		public Vector2 offSet;
		public float offSetX;
		public float offSetY;
		public CollisionType collisionType;
		[HideInInspector]
		public int state;
		[HideInInspector]
		public float storedRadius;
	}
}
