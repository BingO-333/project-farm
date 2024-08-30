using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game
{
    public class SellItemsZone : InteractableZone, ISaveable
    {
        public event Action OnStartReload;
        public event Action OnStopReload;

        [field: SerializeField] public UpgradeLevel Upgrades { get; private set; }

        public string PrefsBaseTag => "SellZone_";

        [SerializeField] int _baseLimit = 10;
        [SerializeField] int _limitPerUpgrade = 5;
        [SerializeField] int _decreaseReloadingPerUpgrade = 5;

        [SerializeField] float _reloadingDuration = 60f;
        [Space]
        [SerializeField] RectTransform _carIcon;
        [SerializeField] Image _reloadingFillImage;
        [SerializeField] TextMeshProUGUI _remainSecondsDisplay;
        [SerializeField] TextMeshProUGUI _moneyForSellingDisplay;

        [Inject] UIManager _uiManager;
        [Inject] MoneyManager _moneyManager;

        private Tween _reloadingTween;

        private Coroutine _interactCoroutine;

        private int _savedMoneyCost
        {
            get => PlayerPrefs.GetInt(GetSaveKey() + "_savedCost", 0);
            set => PlayerPrefs.SetInt(GetSaveKey() + "_savedCost", value);
        }

        private bool _isReloading;

        protected override void Awake()
        {
            base.Awake();

            _remainSecondsDisplay.gameObject.SetActive(false);

            _reloadingFillImage.fillAmount = 1f;
            _uiManager.SellingPanel.OnSell += StartReloading;

            _moneyForSellingDisplay.gameObject.SetActive(false);

            _moneyManager.AddMoney(_savedMoneyCost);
            _savedMoneyCost = 0;
        }

        public string GetSaveKey() => name;
        public void SetSaveKey(string key) => name = key;

        protected override void StartInteract(Player player)
        {
            if (_isReloading)
                return;

            if (_interactCoroutine != null)
                StopCoroutine(_interactCoroutine);

            _interactCoroutine = StartCoroutine(Interacting(player));
        }

        protected override void StopInteract(Player player)
        {
            if (_interactCoroutine != null)
                StopCoroutine(_interactCoroutine);

            _uiManager.SellingPanel.StartHiding();
        }

        private void StartReloading(int totalCost)
        {
            _isReloading = true;

            OnStartReload?.Invoke();

            _savedMoneyCost = totalCost;

            _remainSecondsDisplay.gameObject.SetActive(true);
            _moneyForSellingDisplay.gameObject.SetActive(true);

            _moneyForSellingDisplay.text = totalCost.ToStringWithAbbreviations();

            float reloading = _reloadingDuration - _decreaseReloadingPerUpgrade * Upgrades.Level;

            _reloadingTween.KillIfActiveAndPlaying();
            _reloadingTween = DOVirtual.Float(0, reloading, reloading, (value) =>
            {
                float percent = Mathf.InverseLerp(0, reloading, value);

                //_reloadingFillImage.fillAmount = percent;
                _remainSecondsDisplay.text = Mathf.CeilToInt(reloading - value).ToString();

                Vector3 carIconPosFarm = _carIcon.localPosition;
                Vector3 carIconPosStore = _carIcon.localPosition;
                carIconPosFarm.x = -1f;
                carIconPosStore.x = 1f;

                if (percent < 0.5f)
                {
                    _carIcon.localPosition = Vector3.Lerp(carIconPosFarm, carIconPosStore, percent * 2f);
                    _carIcon.localScale = new Vector3(1f, 1f, 1f);
                }
                else
                {
                    _carIcon.localPosition = Vector3.Lerp(carIconPosStore, carIconPosFarm, (percent - 0.5f) * 2f);
                    _carIcon.localScale = new Vector3(-1f, 1f, 1f);
                }
            })
                .SetEase(Ease.Linear)
                .OnComplete(() => {
                    _isReloading = false;
                    _remainSecondsDisplay.gameObject.SetActive(false);

                    _moneyManager.AddMoney(_savedMoneyCost);
                    _savedMoneyCost = 0;

                    _moneyForSellingDisplay.gameObject.SetActive(false);

                    OnStopReload();
                });
        }

        private IEnumerator Interacting(Player player)
        {
            yield return new WaitUntil(() => player.Movement.IsMoving == false);

            _uiManager.SellingPanel.StartShowing();
            _uiManager.SellingPanel.Setup(player.Inventory, _baseLimit + _limitPerUpgrade * Upgrades.Level);
        }        
    }
}

