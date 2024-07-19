using UnityEngine;
using Obi;
using Sirenix.OdinInspector;

namespace Game
{
    [RequireComponent(typeof(ObiRope))]
    public class HosePumpAnimation : MonoBehaviour
    {
        [SerializeField] GrassSpawner _grassSpawner;

        [Header("Bulge controls")]
        [SerializeField] float _pumpSpeed = 5f;
        [SerializeField] float _bulgeFrequency = 3;
        [SerializeField] float _baseThickness = 0.04f;
        [SerializeField] float _bulgeThickness = 0.06f;
        [SerializeField] float _resetSpeed = 2f;

        private ObiRope _rope;

        private float _time = 0;
        private float _resetTime = 0;

        private bool _isAnimating = false;
        private bool _isResetting = false;

        void OnEnable()
        {
            _rope = GetComponent<ObiRope>();
            _rope.OnBeginStep += Rope_OnBeginStep;

            _grassSpawner.OnStartLoading += StartAnimation;
            _grassSpawner.OnStopLoading += StopAnimation;
        }

        void OnDisable()
        {
            _rope.OnBeginStep -= Rope_OnBeginStep;

            _grassSpawner.OnStartLoading -= StartAnimation;
            _grassSpawner.OnStopLoading -= StopAnimation;
        }

        [Button("Start Animation")]
        public void StartAnimation()
        {
            _isAnimating = true;
            _isResetting = false;
        }

        [Button("Stop Animation")]
        public void StopAnimation()
        {
            _isAnimating = false;
            _isResetting = true;
            _resetTime = 0;
        }

        private void Rope_OnBeginStep(ObiActor actor, float stepTime)
        {
            if (_isAnimating)
            {
                _time += stepTime * _pumpSpeed;

                float distance = 0;
                float sine = 0;

                for (int i = 0; i < _rope.solverIndices.Length; ++i)
                {
                    int solverIndex = _rope.solverIndices[i];

                    if (i > 0)
                    {
                        int previousIndex = _rope.solverIndices[i - 1];
                        distance += Vector3.Distance(_rope.solver.positions[solverIndex], _rope.solver.positions[previousIndex]);
                    }

                    sine = Mathf.Max(0, Mathf.Sin(distance * _bulgeFrequency - _time));

                    _rope.solver.principalRadii[solverIndex] = Vector3.one * Mathf.Lerp(_baseThickness, _bulgeThickness, sine);
                }
            }
            else if (_isResetting)
            {
                _resetTime += stepTime * _resetSpeed;

                for (int i = 0; i < _rope.solverIndices.Length; ++i)
                {
                    int solverIndex = _rope.solverIndices[i];
                    _rope.solver.principalRadii[solverIndex] = Vector3.one * Mathf.Lerp(_rope.solver.principalRadii[solverIndex].x, _baseThickness, _resetTime);
                }

                if (_resetTime >= 1)
                {
                    _isResetting = false;
                    _resetTime = 0;
                }
            }
        }
    }
}
