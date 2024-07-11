using DG.Tweening;
using UnityEngine;

namespace SmartUI 
{
	[RequireComponent(typeof(RectTransform))]
	public class RectMoveElement_SUI : BaseElement_SUI
	{
		[Space]
        [Tooltip("From 'Start position + Show Offset' to 'Start position'")]
        [SerializeField] private Vector2 _showOffset;
        [Tooltip("From 'Start position' to 'Start position + Hide Offset'")]
        [SerializeField] private Vector2 _hideOffset;

		[Space]
		[SerializeField] private Ease _showEasy;
		[SerializeField] private Ease _hideEasy;

		private RectTransform _rectTransform;

		private Vector2 _defaultPosition;

		private void Awake()
		{
			_rectTransform = GetComponent<RectTransform>();

			_defaultPosition = _rectTransform.localPosition;
		}

		protected override Tween ShowAnimation()
		{
			Vector3 startPos = _defaultPosition + _showOffset;

			Tween moveTween = _rectTransform.DOLocalMove(_defaultPosition, _duration)
				.ChangeStartValue(startPos).SetEase(_showEasy);

			return moveTween;
		}

		protected override Tween HideAnimation()
		{
			Vector3 endPos = _defaultPosition + _hideOffset;

			Tween moveTween = _rectTransform.DOLocalMove(endPos, _duration)
				.SetEase(_hideEasy);

			return moveTween;
		}
	}
}