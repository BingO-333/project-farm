using UnityEngine;

namespace Game
{
	public abstract class RandoinPointZone : MonoBehaviour
	{
		public abstract Vector3 GetRandomPointInArea();
		public abstract Vector3 GetRandomPointOnBorder();
	}
}

