using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;

namespace Game
{
	public class MoneyView : MonoBehaviour
	{
        [SerializeField] TextMeshProUGUI _countDisplay;

        [Inject] MoneyManager _moneyManager;

        private Tween _textScaleTween;

        private void Start()
        {
            _moneyManager.OnMoneyCountChanged += UpdateMoneyCount;

            UpdateMoneyCount();
        }

        private void UpdateMoneyCount()
        {
            _countDisplay.text = _moneyManager.Money.ToStringWithAbbreviations();

            _textScaleTween.KillIfActiveAndPlaying();
            _textScaleTween = _countDisplay.transform.DOScale(Vector3.one * 1.3f, 0.1f)
                .ChangeStartValue(Vector3.one)
                .SetLoops(2, LoopType.Yoyo);
        }
    }
}