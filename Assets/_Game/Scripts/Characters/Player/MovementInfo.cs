using UnityEngine;

namespace Game
{
	public abstract class MovementInfo : MonoBehaviour
	{
		public abstract bool IsMoving { get; }
		public abstract Vector3 MoveDirection { get; }

		public abstract void DisableMovement();
		public abstract void EnableMovement();
	}
}