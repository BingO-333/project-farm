using DG.Tweening;
using System;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class HideByBuyingZone : MonoBehaviour
    {
        [SerializeField] TackingMoneyZone[] _tackingMoneyZones;

        private bool _buyingZonesArePurchased => _tackingMoneyZones.Length > 0 ? _tackingMoneyZones.All(b => b.AllMoneyTaken) : true;

        private Tween _anim;

        private void Awake()
        {//
            if (_buyingZonesArePurchased)
                return;

            gameObject.SetActive(false);

            foreach (var bz in _tackingMoneyZones)
                bz.OnMoneyTaken += CheckForActivation;
        }

        private void CheckForActivation()
        {
            if (_buyingZonesArePurchased)
            {
                gameObject.SetActive(true);

                _anim.KillIfActiveAndPlaying();
                _anim = transform.DOScale(Vector3.one, 0.3f).ChangeStartValue(Vector3.zero).SetEase(Ease.OutBack);
            }
        }
    }
}
