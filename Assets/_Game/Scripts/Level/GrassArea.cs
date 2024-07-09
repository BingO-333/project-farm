using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    public class GrassArea : MonoBehaviour
    {
        public event Action<GrassArea> OnEaten;

        public int CurrentHealth { get; private set; }

        [SerializeField] Grass[] _grassPrefabs;
        [SerializeField] float _spawnRadius = 2f;
        [SerializeField] int _health = 10;
        [SerializeField] int _grassCount = 35;

        private List<Grass> _activeGrass = new List<Grass>();

        private void Awake()
        {
            CurrentHealth = _health;
            SpawnGrass();
        }

        public bool TryEat()
        {
            if (CurrentHealth <= 0)
                return false;

            int maxHealth = _health;
            int grassPerEat = _grassCount / maxHealth;
            int extraGrass = _grassCount % maxHealth;

            CurrentHealth--;

            int currentEatCount = CurrentHealth < extraGrass ? grassPerEat + 1 : grassPerEat;

            Sequence grassAnimSequnence = DOTween.Sequence();

            for (int i = 0; i < currentEatCount; i++)
            {
                if (_activeGrass.Count <= 0)
                    break;

                Grass grass = _activeGrass[0];
                _activeGrass.Remove(grass);

                grassAnimSequnence
                    .Join(grass.transform
                        .DOScale(Vector3.zero, 0.15f)
                        .OnComplete(() => grass.gameObject.SetActive(false))
                        .SetDelay(Random.Range(0, 0.1f)));
            }

            if (_activeGrass.Count <= 0)
                OnEaten?.Invoke(this);

            return true;
        }

        private void SpawnGrass()
        {
            for (int i = 0; i < _grassCount; i++)
            {
                Vector3 randomOffset = Random.onUnitSphere * _spawnRadius;
                randomOffset.y = 0;

                Vector3 spawnPos = transform.position + randomOffset;
                Quaternion spawnRot = Quaternion.identity;

                Grass randPrefab = _grassPrefabs[Random.Range(0, _grassPrefabs.Length)];

                Grass spawnedGrass = Instantiate(randPrefab, spawnPos, spawnRot, transform);
                _activeGrass.Add(spawnedGrass);

                spawnedGrass.transform
                        .DOScale(Vector3.one, 0.2f)
                        .ChangeStartValue(Vector3.zero)
                        .SetDelay(Random.Range(0, 0.25f));
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _spawnRadius);
        }
    }
}