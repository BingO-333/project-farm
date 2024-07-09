using UnityEngine;

namespace Game
{
	public class PlayerAnimatorController : CharacterAnimatorController
	{    		
		private Player _player;

        protected override void Awake()
        {
            base.Awake();

            _player = GetComponent<Player>();
        }

        private void Update()
        {
            UpdateMoveState(_player.Movement.MoveDirection.magnitude);
        }
    }
}