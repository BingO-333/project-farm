using DG.Tweening;
using UnityEngine;

namespace SmartUI 
{
	public class BaseElement_SUI : MonoBehaviour
	{
		public ElementStatus_SUI Status { get; private set; }

		public float ShowDelay => _showDelay;
		public float Duration => _duration;

		[SerializeField] protected float _showDelay;
		[SerializeField] protected float _duration = 0.5f;

		[SerializeField] protected bool _unscaledTime;

		private Tween _animationTween;

		private void OnDestroy()
		{
			KillTween();
		}

		public void Show()
		{
			KillTween();

			if (_unscaledTime)
            {
				_animationTween = ShowAnimation()
				.SetDelay(_showDelay)
				.SetUpdate(true)
				.OnComplete(() => Status = ElementStatus_SUI.Shown);
			}
			else
            {
				_animationTween = ShowAnimation()
				.SetDelay(_showDelay)
				.OnComplete(() => Status = ElementStatus_SUI.Shown);
			}
		}

		public void Hide()
		{
			KillTween();

			if (_unscaledTime)
			{
				_animationTween = HideAnimation()
					.SetUpdate(true)
					.OnComplete(() => Status = ElementStatus_SUI.Hidden);
			}
			else 
			{
				_animationTween = HideAnimation()
					.OnComplete(() => Status = ElementStatus_SUI.Hidden);
			}
		}

		protected virtual Tween ShowAnimation() => null;
		protected virtual Tween HideAnimation() => null;

		private void KillTween()
		{
            if (_animationTween.IsActive())
                _animationTween.Kill();
        }
	}
}