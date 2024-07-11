using DG.Tweening;
using UnityEngine;

namespace SmartUI 
{
	[RequireComponent(typeof(CanvasGroup))]
	public class FadeElement_SUI : BaseElement_SUI
	{
		[SerializeField] private Ease _easy;

		private CanvasGroup _canvasGroup;

		private void Awake()
		{
			_canvasGroup = GetComponent<CanvasGroup>();
		}

		protected override Tween ShowAnimation()
		{
			Tween fadeTween = _canvasGroup.DOFade(1, _duration).ChangeStartValue(0).SetEase(_easy);

			return fadeTween;
		}

		protected override Tween HideAnimation()
		{
            Tween fadeTween = _canvasGroup.DOFade(0, _duration).SetEase(_easy);

            return fadeTween;
        }
	}
}