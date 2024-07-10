using System.Collections;
using UnityEngine;

namespace Game
{
	public class ItemModel : TweenBehaviour
	{
		public ItemData Data { get; private set; }

		public void Setup(ItemData itemData)
        {
			Data = itemData;
        }

        private void OnEnable()
        {
            StartCoroutine(AutoDestroyInterval());
        }

        private IEnumerator AutoDestroyInterval()
        {
			yield return new WaitForSeconds(90);

			Destroy(gameObject);
        }
	}
}

