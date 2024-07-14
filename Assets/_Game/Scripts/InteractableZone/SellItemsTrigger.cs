using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game
{
    public class SellItemsTrigger : InteractableZone
    {
        [SerializeField] float _reloadingDuration = 60f;
        [Space]
        [SerializeField] Image _reloadingFillImage;
        [SerializeField] TextMeshProUGUI _remainSecondsDisplay;

        [Inject] UIManager _uiManager;

        private Tween _reloadingTween;

        private Coroutine _interactCoroutine;

        private bool _isReloading;

        protected override void Awake()
        {
            base.Awake();

            _reloadingFillImage.fillAmount = 1f;
            _uiManager.SellingPanel.OnSell += StartReloading;
        }

        protected override void StartInteract(Player player)
        {
            if (_isReloading)
                return;

            if (_interactCoroutine != null)
                StopCoroutine(_interactCoroutine);

            _interactCoroutine = StartCoroutine(Interacting(player));
        }

        protected override void StopInteract(Player player)
        {
            if (_interactCoroutine != null)
                StopCoroutine(_interactCoroutine);
        }

        private void StartReloading()
        {
            _isReloading = true;

            _reloadingTween.KillIfActiveAndPlaying();
            _reloadingTween = DOVirtual.Float(0, _reloadingDuration, _reloadingDuration, (value) =>
            {
                _reloadingFillImage.fillAmount = Mathf.InverseLerp(0, _reloadingDuration, value);
                _remainSecondsDisplay.text = Mathf.CeilToInt(_reloadingDuration - value).ToString();
            })
                .SetEase(Ease.Linear)
                .OnComplete(() => {
                    _isReloading = false;
                    _remainSecondsDisplay.text = _reloadingDuration.ToString();
                });
        }

        private IEnumerator Interacting(Player player)
        {
            yield return new WaitUntil(() => player.Movement.IsMoving == false);

            _uiManager.SellingPanel.StartShowing();
            _uiManager.SellingPanel.Setup(player.Inventory);
        }
    }
}

