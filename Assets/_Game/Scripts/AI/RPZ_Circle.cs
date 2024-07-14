using UnityEngine;

namespace Game
{
    public class RPZ_Circle : RandoinPointZone
    {
        [SerializeField] float _radius = 5f;

        public override Vector3 GetRandomPointInArea()
        {
            Vector2 randomCircle = Random.insideUnitCircle;
            Vector3 randomPoint = new Vector3(randomCircle.x, 0, randomCircle.y);

            return transform.TransformPoint(randomPoint * _radius);
        }

        public override Vector3 GetRandomPointOnBorder()
        {
            Vector2 randomCircle = Random.insideUnitCircle;
            Vector3 randomPoint = new Vector3(randomCircle.x, 0, randomCircle.y);

            return transform.TransformPoint(randomPoint.normalized * _radius);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.gray;
            Gizmos.DrawWireSphere(transform.position, _radius);
        }
    }
}

