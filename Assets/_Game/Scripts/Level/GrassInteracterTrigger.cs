using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(SphereCollider))]
    public class GrassInteracterTrigger : MonoBehaviour
    {
        [SerializeField] float _maxBendAngle = 80f;

        private List<Grass> _grasses = new List<Grass>();

        private SphereCollider _sphereCollider;

        private void Awake()
        {
            _sphereCollider = GetComponent<SphereCollider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Grass grass))
                _grasses.Add(grass);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Grass grass))
                _grasses.Remove(grass);
        }

        private void Update()
        {
            if (_grasses.Count == 0)
                return;

            _grasses.RemoveAll(g => g == null || g.gameObject.activeInHierarchy == false);

            foreach (var grass in _grasses)
                BendGrass(grass);
        }
        
        private void BendGrass(Grass grass)
        {
            float triggerRadius = _sphereCollider.radius;
            float distance = Vector3.Distance(grass.transform.position, transform.position);

            float bendAngle = Mathf.Lerp(0, _maxBendAngle, 1f - (distance / triggerRadius));

            Vector3 direction = (grass.transform.position - transform.position).normalized;
            Quaternion targetRotation = Quaternion.Euler(direction.z * bendAngle, 0, -direction.x * bendAngle);

            grass.transform.rotation = targetRotation;
        }
    }
}