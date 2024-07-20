using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class ItemChanger : InteractableZone
    {
        [SerializeField] Image _fillImage;
        [SerializeField] Transform _spawnContainer;
        [Space]
        [SerializeField] ItemData _inItemData;
        [SerializeField] ItemData _outItemData;
        [Space]
        [SerializeField] float _changingDuration = 10f;

        private Coroutine _gettingItemCoroutine;

        private bool _isChanging;

        protected override void StartInteract(Player player)
        {
            if (_isChanging)
                return;

            if (player.Inventory.GetItemsCount(_inItemData) > 0)
            {
                if (_gettingItemCoroutine != null)
                    StopCoroutine(_gettingItemCoroutine);

                _gettingItemCoroutine = StartCoroutine(GettingItem(player));
            }
        }

        protected override void StopInteract(Player player)
        {
            if (_isChanging)
                return;

            if (_gettingItemCoroutine != null)
                StopCoroutine(_gettingItemCoroutine);
        }

        private void SpawnItem()
        {
            Vector3 randomOffset = Random.insideUnitSphere;
            randomOffset.y = 0;

            Vector3 spawnPos = _spawnContainer.transform.position + randomOffset;

            ItemModel spawnedModel = _outItemData.SpawnItemModel(spawnPos, Quaternion.identity, _spawnContainer);
        }

        private IEnumerator GettingItem(Player player)
        {
            yield return new WaitUntil(() => player.Movement.IsMoving == false);

            if (player.Inventory.GetItemsCount(_inItemData) > 0)
                yield return StartCoroutine(Changing());
        }

        private IEnumerator Changing()
        {
            _isChanging = true;

            for (float progress = 0; progress < 1f; progress += Time.deltaTime / _changingDuration)
            {
                _fillImage.fillAmount = progress;
                yield return null;
            }

            _fillImage.fillAmount = 0;

            SpawnItem();
            _isChanging = false;
        }
    }
}