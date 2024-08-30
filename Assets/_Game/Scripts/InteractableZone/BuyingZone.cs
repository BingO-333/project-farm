using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Game
{
	public class BuyingZone : TackingMoneyZone
	{
		[field: SerializeField] public int DefaultCost { get; private set; } = 500;

        [SerializeField] TackingMoneyProgressView _tackingMoneyProgressView;
		[SerializeField] GameObject _unlockedObj;

        protected override void Awake()
        {
            base.Awake();

            _unlockedObj.SetActive(AllMoneyTaken);

            _tackingMoneyProgressView.gameObject.SetActive(!AllMoneyTaken);

            OnCostChanged += UpdateCostDisplay;
            UpdateCostDisplay();
        }

        protected override void StartInteract(Player player)
        {
			if (AllMoneyTaken)
				return;

            base.StartInteract(player);
        }

		protected override int GetDefaultCost() => DefaultCost;

        protected override void MoneyTaken()
        {
			base.MoneyTaken();

			_unlockedObj.SetActive(true);
            _unlockedObj.transform.DOScale(Vector3.one, 0.25f)
                .ChangeStartValue(new Vector3(1, 0, 1))
                .SetEase(Ease.OutBack);

            _tackingMoneyProgressView.Hide();

			/*VibroManager.Instance.Call(VibroType.NotificationSuccess);
			SFXManager.Instance.BuyObject.Play();*/
		}

		private void UpdateCostDisplay()
        {
			_tackingMoneyProgressView.UpdateCostDisplay(CurrentCost);
        }
    }
}