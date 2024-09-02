using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace Game
{
    public class SellItemsZone : InteractableZone, ISaveable
    {
        public event Action OnStartReload;
        public event Action OnStopReload;

        public event Action<float> OnReloadingUpdate;

        public int SavedCurrentProfit
        {
            get => PlayerPrefs.GetInt(GetSaveKey() + "_savedProfit", 0);
            set => PlayerPrefs.SetInt(GetSaveKey() + "_savedProfit", value);
        }
        public string PrefsBaseTag => "SellZone_";

        [field: SerializeField] public UpgradeLevel Upgrades { get; private set; }

        [SerializeField] int _baseLimit = 10;
        [SerializeField] int _limitPerUpgrade = 5;
        [SerializeField] int _decreaseReloadingPerUpgrade = 5;

        [SerializeField] float _reloadingDuration = 60f;
        [Space]
        [SerializeField] TextMeshProUGUI _remainSecondsDisplay;

        [Inject] UIManager _uiManager;
        [Inject] MoneyManager _moneyManager;

        private Tween _reloadingTween;

        private bool _isReloading;

        protected override void Awake()
        {
            base.Awake();

            _remainSecondsDisplay.gameObject.SetActive(false);

            _uiManager.SellingPanel.OnSell += StartReloading;

            _moneyManager.AddMoney(SavedCurrentProfit);
            SavedCurrentProfit = 0;
        }

        public string GetSaveKey() => name;
        public void SetSaveKey(string key) => name = key;

        protected override void StartInteract(Player player)
        {
            if (_isReloading)
                return;

            _uiManager.SellingPanel.StartShowing();
            _uiManager.SellingPanel.Setup(player.Inventory, _baseLimit + _limitPerUpgrade * Upgrades.Level);
        }

        protected override void StopInteract(Player player)
        {
            _uiManager.SellingPanel.StartHiding();
        }

        private void StartReloading(int totalCost)
        {
            _isReloading = true;

            SavedCurrentProfit = totalCost;

            OnStartReload?.Invoke();

            _remainSecondsDisplay.gameObject.SetActive(true);

            float reloading = _reloadingDuration - _decreaseReloadingPerUpgrade * Upgrades.Level;

            _reloadingTween.KillIfActiveAndPlaying();
            _reloadingTween = DOVirtual.Float(0, reloading, reloading, (value) =>
            {
                float percent = Mathf.InverseLerp(0, reloading, value);
                OnReloadingUpdate?.Invoke(percent);

                _remainSecondsDisplay.text = Mathf.CeilToInt(reloading - value).ToString();

                
            })
                .SetEase(Ease.Linear)
                .OnComplete(() => {
                    _isReloading = false;
                    _remainSecondsDisplay.gameObject.SetActive(false);

                    _moneyManager.AddMoney(SavedCurrentProfit);
                    SavedCurrentProfit = 0;

                    OnStopReload?.Invoke();
                });
        }
    }
}

