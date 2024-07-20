using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace Game
{
	public class Cash : TweenBehaviour
	{
        private Vector3 _standardPos;
        private Vector3 _standardEulerAngles;

        private void Awake()
        {
            _standardPos = transform.localPosition;
            _standardEulerAngles = transform.localEulerAngles;
        }

        public void Show()
        {
            gameObject.SetActive(true);

            SetStartTransform();

            ScaleTo(Vector3.one)
                .SetEase(Ease.OutBack);
        }

        public void SetStartTransform()
        {
            transform.localPosition = _standardPos;
            transform.localEulerAngles = _standardEulerAngles;
        }

        public void MoveToDynamicTarget(Transform target, bool killOnComplete = false)
        {
            _moveTween.KillIfActiveAndPlaying();

            StartCoroutine(MovingToDynamicTarget(target, killOnComplete));
        }

        private IEnumerator MovingToDynamicTarget(Transform target, bool killOnComplete)
        {
            float progress = 0;
            Vector3 startPos = transform.position;
            Vector3 startScale = transform.localScale;

            Quaternion startRot = transform.rotation;
            Quaternion randomRotation = Random.rotation;

            while (progress <= 1)
            {
                progress += Time.deltaTime * 10;
                Vector3 newPos = Vector3.Lerp(startPos, target.position + Vector3.up, progress);
                Vector3 newScale = Vector3.Lerp(startScale, Vector3.one * 0.3f, progress);

                Quaternion newRot = Quaternion.Lerp(startRot, randomRotation, progress);

                transform.position = newPos;
                transform.rotation = newRot;

                //transform.localScale = newScale;

                yield return null;
            }

            if (killOnComplete)
                Destroy(gameObject);
            else
                gameObject.SetActive(false);
        }
    }
}