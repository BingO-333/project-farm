using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Game
{
    public abstract class TackingMoneyZone : InteractableZone, ISaveable
    {
		public bool AllMoneyTaken => CurrentCost <= 0;

		public event Action OnCostChanged;
		public event Action OnMoneyTaken;

		public int CurrentCost
        {
            get => PlayerPrefs.GetInt($"{GetSaveKey()}_currentCost", GetDefaultCost());
            protected set => PlayerPrefs.SetInt($"{GetSaveKey()}_currentCost", value);
        }

		[field: SerializeField] public string PrefsBaseTag { get; private set; } = "TackingMoneyZone";

        [SerializeField] Cash _cashPrefab;

		[Inject] MoneyManager _moneyManager;

		private Coroutine _takeMoneyCoroutine;

		protected override void StartInteract(Player player)
        {
			if (_takeMoneyCoroutine != null)
				StopCoroutine(_takeMoneyCoroutine);

			_takeMoneyCoroutine = StartCoroutine(TakingMoney(player));
		}

        protected override void StopInteract(Player player)
        {
			if (_takeMoneyCoroutine != null)
				StopCoroutine(_takeMoneyCoroutine);
		}

        protected abstract int GetDefaultCost();
		protected virtual void MoneyTaken() 
		{
			OnMoneyTaken?.Invoke();
		}

		private IEnumerator TakingMoney(Player player)
		{
			yield return new WaitUntil(() => player.Movement.IsMoving == false);

			int takeCount = 1;

			while (_moneyManager.Money > 0 && CurrentCost > 0)
			{
				int currentMoney = _moneyManager.Money;

				takeCount = takeCount > CurrentCost ? CurrentCost : takeCount;
				takeCount = takeCount > currentMoney ? currentMoney : takeCount;

				CurrentCost -= takeCount;
				_moneyManager.TryTakeMoney(takeCount);

				Cash spawnedCash = Instantiate(_cashPrefab, player.transform.position + Vector3.up * 2 + Random.insideUnitSphere, Random.rotation);
				spawnedCash.ScaleTo(Vector3.one * 0.4f);
				spawnedCash.JumpTo(TriggerZone.transform.position)
					.OnComplete(() => Destroy(spawnedCash.gameObject));

				takeCount += 5;

				OnCostChanged?.Invoke();

				yield return new WaitForSeconds(0.05f);
			}

			if (CurrentCost <= 0)
				MoneyTaken();
		}

		[Button] protected virtual void ResetPorgress() => CurrentCost = GetDefaultCost();
		[Button] private void Buy() => CurrentCost = 0;

        public string GetSaveKey()
        {
			return transform.name;
        }

        public void SetSaveKey(string key)
        {
			transform.name = key;
        }
    }
}
