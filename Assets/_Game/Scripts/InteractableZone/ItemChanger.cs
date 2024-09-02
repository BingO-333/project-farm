using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class ItemChanger : InteractableZone
    {
        [field: SerializeField] public UpgradeLevel Upgrades { get; private set; }

        [SerializeField] Transform _model;
        [Space]
        [SerializeField] Image _fillImage;
        [SerializeField] Image _iconImage;
        [SerializeField] Transform _spawnContainer;
        [SerializeField] Transform _inputPoint;
        [Space]
        [SerializeField] ItemData _inItemData;
        [SerializeField] ItemData _outItemData;
        [Space]
        [SerializeField] float _changingDuration = 10f;

        private Tweener _modelAnimTweener;

        private Coroutine _gettingItemCoroutine;

        private bool _isChanging;

        protected override void Awake()
        {
            base.Awake();

            _iconImage.sprite = _inItemData.Icon;
            _fillImage.fillAmount = 0;
        }

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
            int itemCount = Mathf.Clamp(Upgrades.Level + 1, 0, player.Inventory.GetItemsCount(_inItemData));
            
            if (itemCount > 0)
            {
                player.Inventory.GetItem(_inItemData, itemCount);

                for (int i = 0; i < itemCount; i++)
                {
                    ItemModel spawnedModel = _inItemData.SpawnItemModel(player.transform.position, Quaternion.identity, transform);
                    spawnedModel.OnTake();

                    spawnedModel.ScaleTo(Vector3.one * 0.3f, 0.5f);
                    spawnedModel.JumpTo(_inputPoint.position, 0.5f, 2f).OnComplete(() => Destroy(spawnedModel.gameObject));

                    yield return new WaitForSeconds(0.2f);
                }

                yield return StartCoroutine(Changing(itemCount));
            }
        }

        private IEnumerator Changing(int itemsCount)
        {
            _isChanging = true;

            _modelAnimTweener.KillIfActiveAndPlaying();
            _modelAnimTweener = _model.DOScale(new Vector3(1, 1.3f, 1f), 0.5f)
                .ChangeStartValue(Vector3.one)
                .SetEase(Ease.InOutQuad)
                .SetLoops(-1, LoopType.Yoyo);

            for (float progress = 0; progress < 1f; progress += Time.deltaTime / _changingDuration)
            {
                _fillImage.fillAmount = progress;
                yield return null;
            }

            _fillImage.fillAmount = 0;

            for (int i = 0; i < itemsCount; i++)
                SpawnItem();

            _modelAnimTweener.KillIfActiveAndPlaying();
            _modelAnimTweener = _model.DOScale(Vector3.one, 0.2f);

            _isChanging = false;
        }
    }
}