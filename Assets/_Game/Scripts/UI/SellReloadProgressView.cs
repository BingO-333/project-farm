using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class SellReloadProgressView : MonoBehaviour
    {
        [SerializeField] SellItemsZone _sellItemsZone;
        [Space]
        [SerializeField] float _carMoveOffset = 1f;

        [SerializeField] RectTransform _carMovePivot;

        [SerializeField] Image _carIcon;
        [SerializeField] Image _moneyIcon;
        [SerializeField] Image _boxIcon;

        [SerializeField] TextMeshProUGUI _moneyForSellingDisplay;

        private Tween _scaleAnim;

        private void Awake()
        {
            _sellItemsZone.OnStartReload += OnStartReload;
            _sellItemsZone.OnStopReload += OnStopReload;

            _sellItemsZone.OnReloadingUpdate += OnReloadingUpdate;

            gameObject.SetActive(false);
        }

        private void OnStartReload()
        {
            _moneyForSellingDisplay.text = _sellItemsZone.SavedCurrentProfit.ToStringWithAbbreviations();

            _boxIcon.gameObject.SetActive(true);
            _moneyIcon.gameObject.SetActive(false);

            gameObject.SetActive(true);

            _scaleAnim.KillIfActiveAndPlaying();
            _scaleAnim = transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutBack).ChangeStartValue(new Vector3(1f, 0, 1f));
        }

        private void OnStopReload()
        {
            _scaleAnim.KillIfActiveAndPlaying();
            _scaleAnim = transform.DOScale(new Vector3(1f, 0, 1f), 0.25f).SetEase(Ease.InBack)
                .OnComplete(() => gameObject.SetActive(false));

            _moneyIcon.gameObject.SetActive(false);
            _boxIcon.gameObject.SetActive(false);
        }

        private void OnReloadingUpdate(float percent)
        {
            Vector3 carIconPosFarm = _carMovePivot.localPosition;
            Vector3 carIconPosStore = _carMovePivot.localPosition;

            carIconPosFarm.x = -_carMoveOffset;
            carIconPosStore.x = _carMoveOffset;

            if (percent < 0.5f)
            {
                _carMovePivot.localPosition = Vector3.Lerp(carIconPosFarm, carIconPosStore, percent * 2f);
                _carIcon.rectTransform.localScale = new Vector3(1f, 1f, 1f);
            }
            else
            {
                _carMovePivot.localPosition = Vector3.Lerp(carIconPosStore, carIconPosFarm, (percent - 0.5f) * 2f);
                _carIcon.rectTransform.localScale = new Vector3(-1f, 1f, 1f);

                _moneyIcon.gameObject.SetActive(true);
                _boxIcon.gameObject.SetActive(false);
            }
        }
    }
}