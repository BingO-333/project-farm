using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game
{
    public class GrassSpawner : InteractableZone
    {
        [SerializeField] float _interactingDuration = 1.5f;
        [SerializeField] float _loadingDuration = 3f;
        [SerializeField] int _cost = 20;
        [SerializeField] int _spawnAreaCount = 3;
        [Space]
        [SerializeField] AnimalField _animalField;
        [Space]
        [SerializeField] Image _interactImageFill;
        [SerializeField] Image _loadingImageFill;
        [SerializeField] TextMeshProUGUI _costDisplay;

        private Coroutine _interactingCoroutine;

        private Tweener _interactFillTweener;
        private Tweener _loadingFillTweener;

        private bool _isLoading;

        [Inject] MoneyManager _moneyManager;

        protected override void Awake()
        {
            base.Awake();

            _costDisplay.text = _cost.ToString();

            _interactImageFill.fillAmount = 0;
            _loadingImageFill.fillAmount = 0;
        }

        protected override void StartInteract(Player player)
        {
            if (_isLoading)
                return;

            if (_moneyManager.Money < _cost)
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

        private IEnumerator Interacting(Player player)
        {
            yield return new WaitUntil(() => player.Movement.IsMoving == false);

            _interactFillTweener.KillIfActiveAndPlaying();
            _interactFillTweener = _interactImageFill.DOFillAmount(1f, _interactingDuration).SetEase(Ease.Linear);

            yield return new WaitForSeconds(_interactingDuration);

            if (_moneyManager.TryTakeMoney(_cost))
                yield return StartCoroutine(Loading());

            _interactFillTweener.KillIfActiveAndPlaying();
            _interactFillTweener = _interactImageFill.DOFillAmount(0, 0.1f);
        }

        private IEnumerator Loading()
        {
            _isLoading = true;

            _loadingFillTweener.KillIfActiveAndPlaying();
            _loadingFillTweener = _loadingImageFill.DOFillAmount(1f, _loadingDuration).SetEase(Ease.Linear);

            yield return new WaitForSeconds(_loadingDuration);

            _loadingFillTweener.KillIfActiveAndPlaying();
            _loadingFillTweener = _loadingImageFill.DOFillAmount(0, 0.1f);

            for (int i = 0; i < _spawnAreaCount; i++)
                _animalField.TrySpawnGrassArea();

            _isLoading = false;
        }
    }
}

