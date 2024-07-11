using DG.Tweening;
using UnityEngine;

namespace SmartUI 
{
	public class RotationElement_SUI : BaseElement_SUI
	{
		[Space]
        [Tooltip("From 'Start Z rotation + Show Z Offset' to 'Start Z Rotation'")]
        [SerializeField] private float _showRotateZOffset;
        [Tooltip("From 'Current Z Rotation' to 'Start Z rotation + Show Z Offset'")]
        [SerializeField] private float _hideRotateZOffset;

		[Space]
		[SerializeField] private Ease _showEasy;
		[SerializeField] private Ease _hideEase;

		[SerializeField] private RotateMode _rotateMode = RotateMode.FastBeyond360;

		private float _startZRotation;

		private void Awake()
		{
			_startZRotation = transform.localEulerAngles.z;
		}

		protected override Tween ShowAnimation()
		{
			Vector3 endAngles = new Vector3(0, 0, _startZRotation);
			Vector3 startAngles = new Vector3(0, 0, _startZRotation + _showRotateZOffset);

			Tween rotateTween = transform.DOLocalRotate(endAngles, Duration, _rotateMode)
				.ChangeStartValue(startAngles).SetEase(_showEasy);

			return rotateTween;
		}

		protected override Tween HideAnimation()
		{
			Vector3 endAngles = new Vector3(0, 0, _startZRotation + _hideRotateZOffset);

			Tween rotateTween = transform.DOLocalRotate(endAngles, Duration, _rotateMode)
				.SetEase(_hideEase);

			return rotateTween;
        }
	}
}