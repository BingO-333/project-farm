using System.Collections;
using UnityEngine;

namespace Game
{
	public class ItemModel : TweenBehaviour
	{
		public ItemData Data { get; private set; }

        private Collider _collider;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
        }

        private void OnEnable()
        {
            StartCoroutine(AutoDestroyInterval());
        }

        public void Setup(ItemData itemData)
        {
			Data = itemData;
        }

        public void DisableCollider()
        {
            _collider.enabled = false;
        }

        private IEnumerator AutoDestroyInterval()
        {
			yield return new WaitForSeconds(90);

			Destroy(gameObject);
        }
	}
}

