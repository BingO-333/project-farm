using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Game
{
	public class MoneyManager : MonoBehaviour
	{   
		public int Money
        {
			get => PlayerPrefs.GetInt("Money", _startMoney);
			private set => PlayerPrefs.SetInt("Money", value);
        }

		public event Action OnMoneyCountChanged;

		public event Action OnMoneyAdd;
		public event Action OnMoneyTaken;

		public event Action OnFailedTakeMoney;

		[SerializeField] int _startMoney = 15;

        [Button] public void AddMoney(int count = 500)
        {
			Money += count;

			OnMoneyAdd?.Invoke();
			OnMoneyCountChanged?.Invoke();
        }

		public bool TryTakeMoney(int count)
		{
			if (count > Money)
            {
				OnFailedTakeMoney?.Invoke();
				return false;
			}

			Money -= count;
			OnMoneyTaken?.Invoke();
			OnMoneyCountChanged?.Invoke();

			return true;
		}
	}
}