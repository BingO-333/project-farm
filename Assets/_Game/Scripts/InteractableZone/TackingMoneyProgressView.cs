using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Game 
{
	public class TackingMoneyProgressView : MonoBehaviour
	{
        //[SerializeField] private Image _fillImage;
        [SerializeField] private TextMeshProUGUI _currentCostDisplay;

        private Tweener _scaleTweener;

        private void OnDestroy()
        {
            _scaleTweener.KillIfActiveAndPlaying();
        }

        public void Hide()
        {
            _scaleTweener.KillIfActiveAndPlaying();
            _scaleTweener = transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack)
                .OnComplete(() => gameObject.SetActive(false));
        }

        public void Show()
        {
            gameObject.SetActive(true);

            _scaleTweener = transform.DOScale(Vector3.one, 0.5f).ChangeStartValue(new Vector3(0, 1, 1)).SetEase(Ease.OutBack);
        }

        public void UpdateCostDisplay(int cost)
        {
            _currentCostDisplay.text = cost.ToStringWithAbbreviations();
            //_fillImage.fillAmount = 1f - Mathf.InverseLerp(0, _buyingZone.DefaultCost, _buyingZone.CurrentCost);
        }
    }
}