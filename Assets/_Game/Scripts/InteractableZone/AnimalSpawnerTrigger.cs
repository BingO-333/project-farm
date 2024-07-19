using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
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
        [SerializeField] int _maxAnimals = 10;

        [SerializeField] AnimalBehaviour _animalPrefab;
        [Space]
        [SerializeField] AnimalField _animalField;
        [SerializeField] Image _interactImageFill;
        [SerializeField] TextMeshProUGUI _costDisplay;
        [SerializeField] TextMeshProUGUI _animalsCountDisplay;

        private List<AnimalBehaviour> _spawnedAnimals = new List<AnimalBehaviour>();

        private Coroutine _interactingCoroutine;

        private Tweener _interactFillTweener;

        private int _savedSpawnedAnimalsCount
        {
            get => PlayerPrefs.GetInt("SpawnedAnimals_" + name, 0);
            set => PlayerPrefs.SetInt("SpawnedAnimals_" + name, value);
        }

        [Inject] MoneyManager _moneyManager;

        protected override void Awake()
        {
            base.Awake();

            _costDisplay.text = _cost.ToStringWithAbbreviations();

            _interactImageFill.fillAmount = 0;

            for (int i = 0; i < _savedSpawnedAnimalsCount; i++)
                TrySpawnAnimal(true);

            _animalsCountDisplay.text = $"{_spawnedAnimals.Count}/{_maxAnimals}";
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

        private void TrySpawnAnimal(bool ignoreSaveCount = false)
        {
            if (_spawnedAnimals.Count < _maxAnimals)
            {
                AnimalBehaviour spawnedAnimal = Instantiate(_animalPrefab, transform.position, Quaternion.identity);
                spawnedAnimal.Setup(_animalField);

                _spawnedAnimals.Add(spawnedAnimal);

                _animalsCountDisplay.text = $"{_spawnedAnimals.Count}/{_maxAnimals}";

                if (ignoreSaveCount == false)
                    _savedSpawnedAnimalsCount = _spawnedAnimals.Count;
            }
        }

        private IEnumerator Interacting(Player player)
        {
            while (_spawnedAnimals.Count < _maxAnimals)
            {
                yield return new WaitUntil(() => player.Movement.IsMoving == false);

                _interactFillTweener.KillIfActiveAndPlaying();
                _interactFillTweener = _interactImageFill.DOFillAmount(1f, _interactingDuration).SetEase(Ease.Linear);

                yield return new WaitForSeconds(_interactingDuration);

                if (_moneyManager.TryTakeMoney(_cost))
                    TrySpawnAnimal();

                _interactFillTweener.KillIfActiveAndPlaying();
                _interactFillTweener = _interactImageFill.DOFillAmount(0, 0.1f);

                yield return new WaitForSeconds(0.2f);
            }
        }
    }
}

