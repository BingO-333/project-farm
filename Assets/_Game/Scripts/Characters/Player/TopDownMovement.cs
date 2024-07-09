using UnityEngine;

namespace Game
{
	[RequireComponent(typeof(CharacterController))]
	public class TopDownMovement : MovementInfo
	{
		public override bool IsMoving => _moveDirection.magnitude > 0;
		public override Vector3 MoveDirection => _moveDirection;

		public bool MovementEnabled { get; private set; } = true;

		[SerializeField] private float _moveSpeed = 5;
		[SerializeField] private float _rotateSpeed = 4;

        private CharacterController _characterController;

		private Transform _cameraSmoother;

		private Vector3 _joystickDirection =>
			new Vector3(SimpleInput.GetAxis("Horizontal"), 0, SimpleInput.GetAxis("Vertical"));

		private Vector3 _moveDirection;

		private Vector3 _smoothedCameraRelativeDirection;

		private Quaternion _cameraRotateSmootherDirection;

		private void Awake()
		{
			_characterController = GetComponent<CharacterController>();

			_cameraSmoother = new GameObject($"{name}_CameraSmoother").transform;
			_cameraSmoother.transform.parent = Camera.main.transform;
			_cameraSmoother.transform.localPosition = Vector3.zero;
			_cameraSmoother.transform.localRotation = Quaternion.identity;
		}

		private void Update()
		{
			_moveDirection = Vector3.zero;

			if (!MovementEnabled)
				return;

			_moveDirection = _joystickDirection;

			_cameraRotateSmootherDirection = Quaternion.Lerp(_cameraRotateSmootherDirection, Camera.main.transform.rotation, Time.deltaTime);
			_cameraSmoother.rotation = _cameraRotateSmootherDirection;

			Vector3 targetCameraRelativeDirection = _cameraSmoother.TransformDirection(_moveDirection);
			targetCameraRelativeDirection.y = 0;
			targetCameraRelativeDirection.Normalize();

			_smoothedCameraRelativeDirection = targetCameraRelativeDirection;

			Move();
			Rotate();
		}

		public override void EnableMovement()
        {
			MovementEnabled = true;
			_characterController.enabled = true;
        }

		public override void DisableMovement()
        {
			MovementEnabled = false;
			_characterController.enabled = false;
        }

        private void Move()
		{
			Vector3 _gravity = new Vector3(0, -2, 0);

			Vector3 cameraRelativeDirection = _smoothedCameraRelativeDirection * _moveDirection.magnitude;
			_characterController.Move((cameraRelativeDirection + _gravity) * _moveSpeed * Time.deltaTime);

			//_characterController.Move((_moveDirection + _gravity) * _moveSpeed * Time.deltaTime);
		}

		private void Rotate()
		{
			if (_smoothedCameraRelativeDirection.magnitude > 0.1f)
			{
				Quaternion toRot = Quaternion.LookRotation(_smoothedCameraRelativeDirection, Vector3.up);                
				transform.rotation = Quaternion.RotateTowards(transform.rotation, toRot, _rotateSpeed * 100 * Time.deltaTime);
			}
		}
	}
}