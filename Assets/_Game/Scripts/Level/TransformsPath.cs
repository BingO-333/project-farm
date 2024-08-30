using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class TransformsPath : MonoBehaviour
    {
        public Vector3[] PatchPositions => _patchPoints.Select(p => p.position).ToArray();

        [SerializeField] Transform[] _patchPoints;

        [SerializeField] bool _onlySelectedGizmos;
        [SerializeField] Color _gizmosColor = Color.magenta;

        [Button] void GetPointsInChildren() =>
            _patchPoints = GetComponentsInChildren<Transform>().Where(p => p != transform).ToArray();

        private void DrawPatch()
        {
            if (_patchPoints == null || _patchPoints.Length == 0)
                return;

            Gizmos.color = _gizmosColor;

            foreach (var pathPoint in _patchPoints)
            {
                Gizmos.DrawSphere(pathPoint.position, 0.1f);

                Gizmos.matrix = Matrix4x4.TRS(pathPoint.position + pathPoint.forward * 0.2f, pathPoint.rotation, new Vector3(0.1f, 0.05f, 0.25f));
                Gizmos.DrawCube(Vector3.zero, Vector3.one);
                Gizmos.matrix = Matrix4x4.identity;
            }

            for (int i = 0; i < _patchPoints.Length; i++)
            {
                if (i >= _patchPoints.Length - 1)
                    return;

                Transform currentPoint = _patchPoints[i];
                Transform nextPoint = _patchPoints[i + 1];

                Gizmos.DrawLine(currentPoint.position, nextPoint.position);
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (_onlySelectedGizmos)
                DrawPatch();
        }

        private void OnDrawGizmos()
        {
            if (_onlySelectedGizmos == false)
                DrawPatch();
        }
    }
}