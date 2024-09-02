using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    public class CarAnimation : MonoBehaviour
    {
        [SerializeField] SellItemsZone _sellItemTrigger;

        [SerializeField] TweenBehaviour _carModel;
        [SerializeField] GameObject _boxes;
        [SerializeField] Transform[] _wheels;

        [SerializeField] TransformsPath _outPath;
        [SerializeField] TransformsPath _inPath;

        private Sequence _wheelsAnimSequence;

        private void Awake()
        {
            _sellItemTrigger.OnStartReload += StartOutAnimation;
            _sellItemTrigger.OnStopReload += StartInAnimation;

            _boxes.gameObject.SetActive(false);
        }

        [Button]private void StartOutAnimation()
        {
            _boxes.gameObject.SetActive(true);

            _wheelsAnimSequence.KillIfActiveAndPlaying();
            _wheelsAnimSequence = DOTween.Sequence();

            foreach (var wheel in _wheels)
                _wheelsAnimSequence.Join(wheel.DOLocalRotate(new Vector3(360, 0, 0), 0.25f, RotateMode.FastBeyond360)
                    .SetEase(Ease.Linear).ChangeStartValue(Vector3.zero));
            _wheelsAnimSequence.SetLoops(-1, LoopType.Incremental);

            _carModel.MoveAlongPath(_outPath.PatchPositions, 4f, lookForwardPath: true).SetEase(Ease.InQuad)
                .OnComplete(() => _wheelsAnimSequence.KillIfActiveAndPlaying());
        }

        [Button]private void StartInAnimation()
        {
            _boxes.gameObject.SetActive(false);

            _wheelsAnimSequence.KillIfActiveAndPlaying();
            _wheelsAnimSequence = DOTween.Sequence();

            foreach (var wheel in _wheels)
                _wheelsAnimSequence.Join(wheel.DOLocalRotate(new Vector3(360, 0, 0), 0.25f, RotateMode.FastBeyond360)
                    .SetEase(Ease.Linear));
            _wheelsAnimSequence.SetLoops(-1, LoopType.Incremental);

            _carModel.MoveAlongPath(_inPath.PatchPositions, 6f, lookForwardPath: true).SetEase(Ease.OutQuad)
                .OnComplete(() => _wheelsAnimSequence.KillIfActiveAndPlaying());
        }
    }
}