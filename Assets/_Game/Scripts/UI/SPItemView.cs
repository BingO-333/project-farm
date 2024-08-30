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
		[SerializeField] TextMeshProUGUI _nameDisplay;
		[SerializeField] TextMeshProUGUI _countDisplay;
		[SerializeField] TextMeshProUGUI _costOneItemDisplay;

		private ItemData _savedItemData;

		public void Setup(ItemData itemData, int count, Action<ItemData> oneSell, Action<ItemData> allSell)
        {
			_savedItemData = itemData;

			_iconImage.sprite = itemData.Icon;
			_nameDisplay.text = itemData.name;
			_countDisplay.text = "x " + count.ToStringWithAbbreviations();
			_costOneItemDisplay.text = itemData.Cost.ToStringWithAbbreviations();

			OnOneSell = oneSell;
			OnAllSell = allSell;
        }

		public void SellOne() => OnOneSell?.Invoke(_savedItemData);
		public void SellAll() => OnAllSell?.Invoke(_savedItemData);
    }
}