using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Game
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AIBehaviour : MovementInfo
    {
        public override bool IsMoving => _agent.velocity.magnitude >= 0.01f;
        public override Vector3 MoveDirection => -_agent.velocity;

        protected NavMeshAgent _agent;

        private Tweener _lookAtTweener;

        protected virtual void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        public override void DisableMovement()
        {
            StopAllCoroutines();
            _agent.enabled = false;
        }

        public override void EnableMovement()
        {
            _agent.enabled = true;
        }

        public void LookAtSmooth(Vector3 pos, float duration = 0.15f)
        {
            _lookAtTweener.KillIfActiveAndPlaying();
            _lookAtTweener = transform.DOLookAt(pos, duration);
        }

        protected IEnumerator Moving(Vector3 pos, int areaMask = NavMesh.AllAreas)
        {
            if (_agent.isOnNavMesh == false)
                yield break;

            _lookAtTweener.KillIfActiveAndPlaying();

            Vector3 samplePos = pos;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(pos, out hit, 50f, 1 << areaMask))
                samplePos = hit.position;

            _agent.SetDestination(samplePos);

            //yield return StartCoroutine(WaitingForAgent());
            yield return null;//for calculate path
            yield return new WaitUntil(() => _agent.remainingDistance <= _agent.stoppingDistance);

            _agent.SetDestination(transform.position);
        }

        protected IEnumerator Following(Transform target, int areaMask = NavMesh.AllAreas)
        {
            _lookAtTweener.KillIfActiveAndPlaying();

            while (enabled)
            {
                Vector3 samplePos = target.position;

                if (NavMesh.SamplePosition(target.position, out NavMeshHit hit, 50f, 1 << areaMask))
                    samplePos = hit.position;

                _agent.SetDestination(samplePos);

                yield return new WaitForSeconds(0.1f);
            }
        }

    }
}
