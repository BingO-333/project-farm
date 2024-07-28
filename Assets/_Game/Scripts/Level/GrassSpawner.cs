using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game
{
    public class GrassSpawner : InteractableZone
    {
        public event Action OnStartLoading;
        public event Action OnStopLoading;

        [field: SerializeField] public UpgradeLevel Upgrades { get; private set; }

        [SerializeField] float _interactingDuration = 1.5f;
        [SerializeField] float _loadingDuration = 3f;
        [SerializeField] int _baseCost = 10;
        [Space]
        [SerializeField] AnimalField _animalField;
        [Space]
        [SerializeField] Image _interactImageFill;
        [SerializeField] Image _loadingImageFill;
        [SerializeField] TextMeshProUGUI _costDisplay;

        [SerializeField] PompAnimation _pompAnimation;
        [SerializeField] RectTransform _wave;

        private Coroutine _interactingCoroutine;

        private Tweener _interactFillTweener;
        private Tweener _loadingFillTweener;

        private bool _isLoading;

        private int _currentCost => (Upgrades.Level + 1) * _baseCost;

        [Inject] MoneyManager _moneyManager;

        protected override void Awake()
        {
            base.Awake();

            Upgrades.OnLevelUp += UpdateCostDisplay;
            UpdateCostDisplay();

            _interactImageFill.fillAmount = 0;
            _loadingImageFill.fillAmount = 0;
        }

        protected override void StartInteract(Player player)
        {
            if (_isLoading)
                return;

            if (_moneyManager.Money < _currentCost)
                return;

            if (_interactingCoroutine != null)
                StopCoroutine(_interactingCoroutine);

            _interactingCoroutine = StartCoroutine(Interacting(player));
        }

        protected override void StopInteract(Player player)
        {
            if (_isLoading)
                return;

            if (_interactingCoroutine != null)
                StopCoroutine(_interactingCoroutine);

            _interactFillTweener.KillIfActiveAndPlaying();
            _interactFillTweener = _interactImageFill.DOFillAmount(0, 0.1f);
        }

        private void UpdateCostDisplay()
        {
            _costDisplay.text = _currentCost.ToString();
        }

        private IEnumerator Interacting(Player player)
        {
            yield return new WaitUntil(() => player.Movement.IsMoving == false);

            _interactFillTweener.KillIfActiveAndPlaying();
            _interactFillTweener = _interactImageFill.DOFillAmount(1f, _interactingDuration).SetEase(Ease.Linear);

            yield return new WaitForSeconds(_interactingDuration);

            if (_moneyManager.TryTakeMoney(_currentCost))
                yield return StartCoroutine(Loading((Upgrades.Level + 1)));

            _interactFillTweener.KillIfActiveAndPlaying();
            _interactFillTweener = _interactImageFill.DOFillAmount(0, 0.1f);
        }

        private IEnumerator Loading(int grassCount)
        {
            _isLoading = true;
            OnStartLoading?.Invoke();

            _loadingFillTweener.KillIfActiveAndPlaying();
            _loadingFillTweener = DOVirtual.Float(0, 1f, _loadingDuration, v => {
                _loadingImageFill.fillAmount = v;

                Vector3 wavePos = Vector3.zero;
                wavePos.y = Mathf.Lerp(-1f, 1f, v);
                wavePos.x = Mathf.Lerp(-4f, 4f, v);

                _wave.localPosition = wavePos;
            }).SetEase(Ease.Linear);

            _pompAnimation.StartAnimation();

            yield return new WaitForSeconds(_loadingDuration);

            _pompAnimation.StopAnimation();

            _loadingFillTweener.KillIfActiveAndPlaying();
            _loadingFillTweener = DOVirtual.Float(1f, 0, 0.1f, v => {
                _loadingImageFill.fillAmount = v;

                Vector3 wavePos = Vector3.zero;
                wavePos.y = Mathf.Lerp(-1f, 1f, v);
                wavePos.x = Mathf.Lerp(-2f, 2f, v);

                _wave.localPosition = wavePos;
            }).SetEase(Ease.Linear);

            for (int i = 0; i < grassCount; i++)
                _animalField.TrySpawnGrassArea();

            _isLoading = false;
            OnStopLoading?.Invoke();
        }
    }
}

