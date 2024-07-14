using UnityEngine;

namespace Game
{
    public class RPZ_Quad : RandoinPointZone
    {
        [SerializeField] Vector2 _size = Vector2.one;

        public override Vector3 GetRandomPointInArea()
        {
            Vector3 randomPoint = Vector3.zero;

            randomPoint.x += Random.Range(-_size.x / 2, _size.x / 2);
            randomPoint.z += Random.Range(-_size.y / 2, _size.y / 2);

            return transform.TransformPoint(randomPoint);
        }

        public override Vector3 GetRandomPointOnBorder()
        {
            bool horizontalBorder = Random.Range(0, 2) == 0;

            Vector3 randomPoint = transform.position;

            if (horizontalBorder)
            {
                randomPoint.x += Random.Range(0, 2) == 0 ? _size.x / 2 : _size.x / 2 * -1f;
                randomPoint.z += Random.Range(-_size.y / 2, _size.y / 2);
            }
            else
            {
                randomPoint.x += Random.Range(-_size.x / 2, _size.x / 2);
                randomPoint.z += Random.Range(0, 2) == 0 ? _size.y / 2 : _size.y / 2 * -1f;
            }

            return randomPoint;
        }

        private void OnDrawGizmosSelected()
        {
            Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);
            Gizmos.matrix = rotationMatrix;

            Color color = Color.gray;
            color.a = 0.1f;

            Gizmos.color = color;
            Gizmos.DrawCube(Vector3.zero, new Vector3(_size.x, 0.05f, _size.y));
        }
    }
}
