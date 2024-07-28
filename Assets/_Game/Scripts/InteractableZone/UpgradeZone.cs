using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Game
{
    public class UpgradeZone : TackingMoneyZone
    {
		[field: SerializeField] public int DefaultCost { get; private set; } = 500;

		[SerializeField] TackingMoneyProgressView _tackingMoneyProgressView;

		private void Start()
		{
			if (AllMoneyTaken)
            {
				gameObject.SetActive(false);
				return;
            }

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

			_tackingMoneyProgressView.Hide();

			TriggerZone.gameObject.SetActive(false);
			transform.DOScale(Vector3.zero, 0.25f)
				.OnComplete(() => gameObject.SetActive(false));

			/*VibroManager.Instance.Call(VibroType.NotificationSuccess);
			SFXManager.Instance.BuyObject.Play();*/
		}

		private void UpdateCostDisplay()
		{
			_tackingMoneyProgressView.UpdateCostDisplay(CurrentCost);
		}
	}
}
