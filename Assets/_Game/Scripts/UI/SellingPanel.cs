using SmartUI;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game
{
	public class SellingPanel : Page_SUI
	{
		public event Action OnSell;

		[SerializeField] SPItemView _inInvItemViewPrefab;
		[SerializeField] SPItemView _forSellItemViewPrefab;

		[SerializeField] Transform _inInvContainer;
		[SerializeField] Transform _forSellContainer;

		private Dictionary<ItemData, SPItemView> _inInvItemViews = new Dictionary<ItemData, SPItemView>();
		private Dictionary<ItemData, SPItemView> _forSellItemViews = new Dictionary<ItemData, SPItemView>();

		private Dictionary<ItemData, int> _itemsForSelling = new Dictionary<ItemData, int>();

		private Inventory _savedInventory;

		[Inject] MoneyManager _moneyManager;

		public void Setup(Inventory inventory)
        {
			_itemsForSelling.Clear();

			_savedInventory = inventory;

			UpdateInInvItemViews();
			UpdateForSellItemViews();
        }

		public void Sell()
        {
            foreach (var keyValueItem in _itemsForSelling)
            {
				ItemData data = keyValueItem.Key;
				int count = keyValueItem.Value;

                for (int i = 0; i < count; i++)
					_savedInventory.GetItem(data);

				_moneyManager.AddMoney(data.Cost * count);
            }

			_itemsForSelling.Clear();
			StartHiding();

			OnSell?.Invoke();
        }

		public void Close()
        {
			_itemsForSelling.Clear();
			StartHiding();
        }

		private void UpdateInInvItemViews()
        {
			foreach (var itemView in _inInvItemViews.Values)
				itemView.gameObject.SetActive(false);

            foreach (var keyValueItem in _savedInventory.Items)
            {
				ItemData data = keyValueItem.Key;
				int count = keyValueItem.Value;

				if (_itemsForSelling.ContainsKey(data))
					count -= _itemsForSelling[data];

				if (count <= 0)
					continue;

				if (_inInvItemViews.ContainsKey(data) == false)
					_inInvItemViews.Add(data, Instantiate(_inInvItemViewPrefab, _inInvContainer));

				SPItemView itemView = _inInvItemViews[data];
				itemView.Setup(data, count, AddOneItemForSell, AddAllItemsForSell);

				itemView.gameObject.SetActive(true);
            }
        }

		private void UpdateForSellItemViews()
        {
			foreach (var itemView in _forSellItemViews.Values)
				itemView.gameObject.SetActive(false);

            foreach (var keyValueItem in _itemsForSelling)
            {
				ItemData data = keyValueItem.Key;
				int count = keyValueItem.Value;

				if (_forSellItemViews.ContainsKey(data) == false)
					_forSellItemViews.Add(data, Instantiate(_forSellItemViewPrefab, _forSellContainer));

				SPItemView itemView = _forSellItemViews[data];
				itemView.Setup(data, count, RemoveOneItemFromSell, RemoveAllItemsFromSell);

				itemView.gameObject.SetActive(true);
            }
        }

		private void AddOneItemForSell(ItemData data)
        {
			if (_savedInventory.GetItemsCount(data) <= 0)
				return;

			if (_itemsForSelling.ContainsKey(data) == false)
				_itemsForSelling.Add(data, 0);

			_itemsForSelling[data]++;

			UpdateInInvItemViews();
			UpdateForSellItemViews();
        }

		private void AddAllItemsForSell(ItemData data)
        {
			if (_savedInventory.GetItemsCount(data) <= 0)
				return;

			if (_itemsForSelling.ContainsKey(data) == false)
				_itemsForSelling.Add(data, 0);

			_itemsForSelling[data] = _savedInventory.GetItemsCount(data);

			UpdateInInvItemViews();
			UpdateForSellItemViews();
		}

		private void RemoveOneItemFromSell(ItemData data)
        {
			if (_itemsForSelling.ContainsKey(data) == false)
				return;

			_itemsForSelling[data]--;

			if (_itemsForSelling[data] <= 0)
				_itemsForSelling.Remove(data);

			UpdateInInvItemViews();
			UpdateForSellItemViews();
		}

		private void RemoveAllItemsFromSell(ItemData data)
        {
			if (_itemsForSelling.ContainsKey(data) == false)
				return;

			_itemsForSelling.Remove(data);

			UpdateInInvItemViews();
			UpdateForSellItemViews();
		}
    }
}

