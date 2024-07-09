using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class AnimalField : MonoBehaviour
    {
        public event Action OnGrassAreaCountChanged;
        public int GrassAreaCount => _spawnedGrassAreas.Count;

        [field: SerializeField] RandomPointZone _randomPointZone;

        [SerializeField] GrassArea _grassAreaPrefab;

        [SerializeField] int _maxGrassAreas = 20;

        private List<GrassArea> _spawnedGrassAreas = new List<GrassArea>();

        [Button] public void TrySpawnGrassArea()
        {
            if (_spawnedGrassAreas.Count >= _maxGrassAreas)
                return;

            Vector3 pos = _randomPointZone.GetRandomPointInArea();

            GrassArea spawnedGrassArea = Instantiate(_grassAreaPrefab, pos, Quaternion.identity, transform);
            spawnedGrassArea.OnEaten += RemoveGrassArea;

            _spawnedGrassAreas.Add(spawnedGrassArea);
            OnGrassAreaCountChanged?.Invoke();
        }

        private void RemoveGrassArea(GrassArea grassArea)
        {
            _spawnedGrassAreas.Remove(grassArea);
            OnGrassAreaCountChanged?.Invoke();
        }
    }
}