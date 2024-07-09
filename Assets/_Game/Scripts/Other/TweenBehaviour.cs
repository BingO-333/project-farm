using DG.Tweening;
using UnityEngine;

namespace Game
{
	public class TweenBehaviour : MonoBehaviour
	{
		protected Tween _moveTween;
		protected Tween _rotateTween;
		protected Tween _scaleTween;

        private float _defaultAnimDuration = 0.4f;

        private void OnDestroy()
		{
			_moveTween.KillIfActiveAndPlaying();
			_rotateTween.KillIfActiveAndPlaying();
			_scaleTween.KillIfActiveAndPlaying();
		}

        public Tween MoveTo(Vector3 pos)
        {
            if (_moveTween.IsActive())
                _moveTween.Kill();

            return _moveTween = transform.DOMove(pos, _defaultAnimDuration);
        }

        public Tween LocalMoveTo(Vector3 pos)
        {
            if (_moveTween.IsActive())
                _moveTween.Kill();

            return _moveTween = transform.DOLocalMove(pos, _defaultAnimDuration);
        }

        public Tween JumpTo(Vector3 pos)
        {
            if (_moveTween.IsActive())
                _moveTween.Kill();

            return _moveTween = transform.DOJump(pos, 2f, 1, _defaultAnimDuration);
        }

        public Tween LocalJumpTo(Vector3 pos)
        {
            if (_moveTween.IsActive())
                _moveTween.Kill();

            return _moveTween = transform.DOLocalJump(pos, 2f, 1, _defaultAnimDuration);
        }

        public Tween ScaleTo(Vector3 scale)
        {
            if (_scaleTween.IsActive())
                _scaleTween.Kill();

            return _scaleTween = transform.DOScale(scale, _defaultAnimDuration);
        }

        public Tween RotateTo(Vector3 angles)
        {
            if (_rotateTween.IsActive())
                _rotateTween.Kill();

            return _rotateTween = transform.DORotate(angles, _defaultAnimDuration);
        }

        public Tween LocaleRotateTo(Vector3 angles)
        {
            if (_rotateTween.IsActive())
                _rotateTween.Kill();

            return _rotateTween = transform.DOLocalRotate(angles, _defaultAnimDuration);
        }
    }
}