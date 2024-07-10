using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game
{
    public class AnimalSpawnerTrigger : InteractableZone
    {
        [SerializeField] float _interactingDuration = 1.5f;
        [SerializeField] int _cost = 20;

        [SerializeField] AnimalBehaviour _animalPrefab;
        [Space]
        [SerializeField] AnimalField _animalField;
        [SerializeField] Image _interactImageFill;
        [SerializeField] TextMeshProUGUI _costDisplay;

        private Coroutine _interactingCoroutine;

        private Tweener _interactFillTweener;

        [Inject] MoneyManager _moneyManager;

        protected override void Awake()
        {
            base.Awake();

            _costDisplay.text = _cost.ToStringWithAbbreviations();

            _interactImageFill.fillAmount = 0;
        }

        protected override void StartInteract(Player player)
        {
            if (_moneyManager.Money < _cost)
                return;

            if (_interactingCoroutine != null)
                StopCoroutine(_interactingCoroutine);

            _interactingCoroutine = StartCoroutine(Interacting(player));
        }

        protected override void StopInteract(Player player)
        {
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

            if (_moneyManager.Money >= _cost)
            {
                AnimalBehaviour spawnedAnimal = Instantiate(_animalPrefab, transform.position, Quaternion.identity);
                spawnedAnimal.Setup(_animalField);
            }

            _interactFillTweener.KillIfActiveAndPlaying();
            _interactFillTweener = _interactImageFill.DOFillAmount(0, 0.1f);
        }
    }
}

