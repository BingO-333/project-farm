using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game
{
	public class InventoryView : MonoBehaviour
	{
		[SerializeField] InvItemView _invItemViewPrefab;
		[SerializeField] Transform _container;

        private Dictionary<ItemData, InvItemView> _spawnedInvItemViews = new Dictionary<ItemData, InvItemView>();

		[Inject] Player _player;

        private void Start()
        {
            _player.Inventory.OnAddItem += UpdateView;
            _player.Inventory.OnRemoveItem += UpdateView;

            UpdateView();
        }

        private void UpdateView()
        {
            foreach (var view in _spawnedInvItemViews.Values)
                view.gameObject.SetActive(false);

            if (_player.Inventory.Items.Count <= 0)
            {
                _container.gameObject.SetActive(false);
                return;
            }

            _container.gameObject.SetActive(true);

            foreach (var itemKeyValue in _player.Inventory.Items)
            {
                ItemData data = itemKeyValue.Key;
                int count = itemKeyValue.Value;

                if (_spawnedInvItemViews.ContainsKey(data) == false)
                {
                    InvItemView spawnedView = Instantiate(_invItemViewPrefab, _container);
                    _spawnedInvItemViews.Add(data, spawnedView);
                }

                InvItemView view = _spawnedInvItemViews[data];

                view.Setup(data, count);
                view.gameObject.SetActive(true);
            }
        }
    }
}