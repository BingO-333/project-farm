using DG.Tweening;
using UnityEngine;

namespace Game
{
	public class TweenBehaviour : MonoBehaviour
	{
        protected Tween _moveTween;
        protected Tween _rotateTween;
        protected Tween _scaleTween;

        private void OnDestroy()
        {
            _moveTween.KillIfActiveAndPlaying();
            _rotateTween.KillIfActiveAndPlaying();
            _scaleTween.KillIfActiveAndPlaying();
        }

        public void StopRotating() => _rotateTween.KillIfActiveAndPlaying();
        public void StopMoving() => _moveTween.KillIfActiveAndPlaying();
        public void StopScaling() => _scaleTween.KillIfActiveAndPlaying();

        public Tween ShakeRotation(float duration = 0.2f, float strenght = 90f, int vibrato = 9)
        {
            _rotateTween.KillIfActiveAndPlaying();
            return _rotateTween = transform.DOShakeRotation(duration, strength: strenght, vibrato: vibrato);
        }

        public Tween MoveTo(Vector3 pos, float duration = 0.2f)
        {
            if (_moveTween.IsActive())
                _moveTween.Kill();

            return _moveTween = transform.DOMove(pos, duration);
        }

        public Tween LocalMoveTo(Vector3 pos, float duration = 0.2f)
        {
            if (_moveTween.IsActive())
                _moveTween.Kill();

            return _moveTween = transform.DOLocalMove(pos, duration);
        }

        public Tween JumpTo(Vector3 pos, float duration = 0.2f, float height = 2f)
        {
            if (_moveTween.IsActive())
                _moveTween.Kill();

            return _moveTween = transform.DOJump(pos, height, 1, duration);
        }

        public Tween LocalJumpTo(Vector3 pos, float duration = 0.2f, float height = 2f)
        {
            if (_moveTween.IsActive())
                _moveTween.Kill();

            return _moveTween = transform.DOLocalJump(pos, height, 1, duration);
        }

        public Tween ScaleTo(Vector3 scale, float duration = 0.2f)
        {
            if (_scaleTween.IsActive())
                _scaleTween.Kill();

            return _scaleTween = transform.DOScale(scale, duration);
        }

        public Tween RotateTo(Vector3 angles, float duration = 0.2f)
        {
            if (_rotateTween.IsActive())
                _rotateTween.Kill();

            return _rotateTween = transform.DORotate(angles, duration);
        }

        public Tween LocaleRotateTo(Vector3 angles, float duration = 0.2f, RotateMode rotateMode = RotateMode.Fast)
        {
            if (_rotateTween.IsActive())
                _rotateTween.Kill();

            return _rotateTween = transform.DOLocalRotate(angles, duration, rotateMode);
        }

    }
}