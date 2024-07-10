using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class AnimalField : MonoBehaviour
    {
        public event Action OnGrassAreaCountChanged;
        public List<GrassArea> GrassAreas { get; private set; } = new List<GrassArea>();

        [field: SerializeField] public RandomPointZone RandomPointZone { get; private set; }

        [SerializeField] GrassArea _grassAreaPrefab;

        [SerializeField] int _maxGrassAreas = 20;

        [Button] public void TrySpawnGrassArea()
        {
            if (GrassAreas.Count >= _maxGrassAreas)
                return;

            Vector3 pos = RandomPointZone.GetRandomPointInArea();

            GrassArea spawnedGrassArea = Instantiate(_grassAreaPrefab, pos, Quaternion.identity, transform);
            spawnedGrassArea.OnEaten += RemoveGrassArea;

            GrassAreas.Add(spawnedGrassArea);
            OnGrassAreaCountChanged?.Invoke();
        }

        private void RemoveGrassArea(GrassArea grassArea)
        {
            GrassAreas.Remove(grassArea);
            OnGrassAreaCountChanged?.Invoke();
        }
    }
}