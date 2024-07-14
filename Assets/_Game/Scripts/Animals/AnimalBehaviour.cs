using System.Collections;
using UnityEngine;

namespace Game
{
	public class AnimalBehaviour : AIBehaviour
	{
        [SerializeField] ItemData _spawnItemData;

        [SerializeField] float _eatingDuration = 3f;
        [SerializeField] float _eatingInterval = 5f;

        [SerializeField] int _eatCountForSpawnItem = 3;

        private AnimalField _animalField;

        private Animator _animator;

        private int _currentEatCount;

        private string _movingAnimKey => "Moving";
        private string _eatingAnimKey => "Eating";

        protected override void Awake()
        {
            base.Awake();
            _animator = GetComponentInChildren<Animator>();
        }

        public void Setup(AnimalField animalField)
        {
            _animalField = animalField;
            StartCoroutine(BehavioursCycle());
        }

        private IEnumerator BehavioursCycle()
        {
            float lastEatingTime = 0;

            while (true)
            {
                if (lastEatingTime + _eatingInterval < Time.time && _animalField.GrassAreas.Count > 0)
                {
                    yield return StartCoroutine(EatingBehaviour());
                    lastEatingTime = Time.time;
                }
                else
                {
                    yield return StartCoroutine(IdleBehaviour());
                }
            }
        }

        private IEnumerator IdleBehaviour()
        {
            _animator.SetBool(_movingAnimKey, true);
            yield return StartCoroutine(Moving(_animalField.RandomPointZone.GetRandomPointInArea()));
            _animator.SetBool(_movingAnimKey, false);

            yield return new WaitForSeconds(Random.Range(0, 5f));
        }

        private IEnumerator EatingBehaviour()
        {
            while (_animalField.GrassAreas.Count > 0)
            {
                GrassArea grassArea = _animalField.GrassAreas[Random.Range(0, _animalField.GrassAreas.Count)];

                Vector3 randomOffset = Random.onUnitSphere * Random.Range(1f, 1.3f);
                randomOffset.y = 0;

                _animator.SetBool(_movingAnimKey, true);
                yield return StartCoroutine(Moving(grassArea.transform.position + randomOffset));
                _animator.SetBool(_movingAnimKey, false);

                if (grassArea != null && grassArea.TryEat())
                {
                    LookAtSmooth(grassArea.transform.position, 0.35f);

                    _animator.SetBool(_eatingAnimKey, true);
                    yield return new WaitForSeconds(_eatingDuration);
                    _animator.SetBool(_eatingAnimKey, false);

                    _currentEatCount++;

                    if (_currentEatCount >= _eatCountForSpawnItem)
                    {
                        _currentEatCount = 0;
                        _spawnItemData.SpawnItemModel(transform.position, Quaternion.identity, null);
                    }

                    break;
                }
            }
        }
    }
}

