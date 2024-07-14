using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
	public class Inventory : MonoBehaviour
	{
		public event Action OnAddItem;
		public event Action OnRemoveItem;

		public Dictionary<ItemData, int> Items { get; private set; } = new Dictionary<ItemData, int>();

		public int GetItemsCount(ItemData itemData)
        {
			if (Items.ContainsKey(itemData) == false)
				return 0;

			return Items[itemData];
        }

		public void AddItem(ItemData itemData, int count = 1)
        {
			if (Items.ContainsKey(itemData) == false)
				Items.Add(itemData, 0);

			Items[itemData] += count;

			OnAddItem?.Invoke();
        }

		public int GetAllItems(ItemData itemData)
        {
			if (Items.ContainsKey(itemData) == false)
				return 0;

			int count = Items[itemData];
			Items.Remove(itemData);

			return count;
        }

		public bool GetItem(ItemData itemData, int count = 1)
        {
			if (Items.ContainsKey(itemData) == false || count > Items[itemData])
				return false;

			Items[itemData] -= count;

			if (Items[itemData] <= 0)
				Items.Remove(itemData);

			OnRemoveItem?.Invoke();

			return true;
        }
	}
}

