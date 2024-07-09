using System;
using UnityEngine;

namespace Game
{
	public class CharacterAnimatorController : MonoBehaviour
	{
		protected Animator _animator;

		protected virtual void Awake()
		{
			_animator = GetComponentInChildren<Animator>();
		}

        protected void UpdateMoveState(float moveSpeed)
		{
			if (_animator == null)
				return;
			
			_animator.SetFloat("MoveState", moveSpeed);
		}
	}
}