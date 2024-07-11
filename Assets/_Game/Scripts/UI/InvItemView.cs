using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
	public class InvItemView : MonoBehaviour
	{
		[SerializeField] Image _iconImage;
		[SerializeField] TextMeshProUGUI _countDisplay;

		public void Setup(ItemData itemData, int count)
        {
			_iconImage.sprite = itemData.Icon;
			_countDisplay.text = count.ToStringWithAbbreviations();
        }
	}
}

