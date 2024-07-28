using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace Game
{
	public class ItemModel : TweenBehaviour
	{
		public ItemData Data { get; private set; }

        private Collider _collider;

        private Sequence _idleAnimation;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
        }

        private void OnEnable()
        {
            _idleAnimation.KillIfActiveAndPlaying();

            StartCoroutine(AutoDestroyInterval());

            _idleAnimation = DOTween.Sequence();
            _idleAnimation
                .Append(transform
                    .DOScale(Vector3.one, 0.2f)
                    .ChangeStartValue(Vector3.zero)
                    .SetEase(Ease.OutBack))
                .Append(transform
                    .DOScale(Vector3.one * 1.25f, Random.Range(0.4f, 0.6f))
                    .ChangeStartValue(Vector3.one)
                    .SetLoops(1000, LoopType.Yoyo))
                .Join(transform
                    .DORotate(new Vector3(0, 180f, 0), Random.Range(0.9f, 1.1f), RotateMode.FastBeyond360)
                    .SetLoops(1000, LoopType.Incremental).SetEase(Ease.Linear));
        }

        private void OnDisable()
        {
            _idleAnimation.KillIfActiveAndPlaying();
        }

        public void Setup(ItemData itemData)
        {
			Data = itemData;
        }

        public void OnTake()
        {
            _idleAnimation.KillIfActiveAndPlaying();
            _collider.enabled = false;
        }

        private IEnumerator AutoDestroyInterval()
        {
			yield return new WaitForSeconds(90);

			Destroy(gameObject);
        }
	}
}

