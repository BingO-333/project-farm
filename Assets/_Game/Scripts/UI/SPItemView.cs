using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
	public class SPItemView : MonoBehaviour
	{
		public event Action<ItemData> OnOneSell;
		public event Action<ItemData> OnAllSell;

		[SerializeField] Image _iconImage;
		[SerializeField] TextMeshProUGUI _nameCountDisplay;

		private ItemData _savedItemData;

		public void Setup(ItemData itemData, int count, Action<ItemData> oneSell, Action<ItemData> allSell)
        {
			_savedItemData = itemData;

			_iconImage.sprite = itemData.Icon;
			_nameCountDisplay.text = $"{itemData.name} x {count.ToStringWithAbbreviations()}";

			OnOneSell = oneSell;
			OnAllSell = allSell;
        }

		public void SellOne() => OnOneSell?.Invoke(_savedItemData);
		public void SellAll() => OnAllSell?.Invoke(_savedItemData);
    }
}