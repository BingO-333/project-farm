using UnityEngine;

namespace Game
{
    public class AnimalMoveableArea : MonoBehaviour
    {
        [SerializeField] Vector2 _size = Vector2.one;

        public Vector3 GetRandomPoint()
        {
            Vector3 randomPoint = Vector3.zero;

            randomPoint.x += Random.Range(-_size.x / 2, _size.x / 2);
            randomPoint.z += Random.Range(-_size.y / 2, _size.y / 2);

            return transform.TransformPoint(randomPoint);
        }

        private void OnDrawGizmosSelected()
        {
            Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);
            Gizmos.matrix = rotationMatrix;

            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(Vector3.zero, new Vector3(_size.x, 0.05f, _size.y));
        }
    }
}
