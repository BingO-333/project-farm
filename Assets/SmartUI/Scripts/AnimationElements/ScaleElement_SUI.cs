using DG.Tweening;
using UnityEngine;

namespace SmartUI 
{
	public class ScaleElement_SUI : BaseElement_SUI
	{
		[Space]
		[SerializeField] private Ease _showEase;
		[SerializeField] private Ease _hideEase;

		[Space]
		[SerializeField] private Vector3 _showStartScale = Vector3.one;
		[SerializeField] private Vector3 _hideEndScale = Vector3.zero;
		
		protected override Tween ShowAnimation()
		{
			Tween scaleTween = transform.DOScale(Vector3.one, _duration)
				.ChangeStartValue(_showStartScale).SetEase(_showEase);

			return scaleTween;
		}

		protected override Tween HideAnimation()
		{
			Tween scaleTween = transform.DOScale(_hideEndScale, _duration).SetEase(_hideEase);

			return scaleTween;
		}
	}
}