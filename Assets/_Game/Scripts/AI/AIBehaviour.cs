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
        public override bool IsMoving => _agent.velocity.magnitude > 0.01f;
        public override Vector3 MoveDirection => _agent.velocity;

        protected NavMeshAgent _agent;

        private Coroutine _movingCoroutine;

        private Tweener _lookAtTweener;

        protected virtual void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        public override void DisableMovement()
        {
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

        public void MoveTo(Vector3 pos, Action onComplete = null)
        {
            if (_agent.enabled == false)
                return;

            if (_movingCoroutine != null)
                StopCoroutine(_movingCoroutine);

            _lookAtTweener.KillIfActiveAndPlaying();
            _movingCoroutine = StartCoroutine(Moving(pos, onComplete));
        }

        public void FollowTarget(Transform target)
        {
            if (_agent.enabled == false)
                return;
            
            if (_movingCoroutine != null)
                StopCoroutine(_movingCoroutine);

            _lookAtTweener.KillIfActiveAndPlaying();
            _movingCoroutine = StartCoroutine(Following(target));
        }
        
        private IEnumerator Moving(Vector3 pos, Action onComplete)
        {
            Vector3 samplePos = pos;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(pos, out hit, 10f, NavMesh.AllAreas))
                samplePos = hit.position;

            _agent.SetDestination(samplePos);

            yield return new WaitUntil(() => Vector3.Distance(transform.position, samplePos) <= _agent.stoppingDistance + 0.35f);

            _agent.SetDestination(transform.position);

            onComplete?.Invoke();
        }

        private IEnumerator Following(Transform target)
        {
            while (enabled)
            {
                Vector3 samplePos = target.position;

                if (NavMesh.SamplePosition(target.position, out NavMeshHit hit, 10f, NavMesh.AllAreas))
                    samplePos = hit.position;

                _agent.SetDestination(samplePos);

                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
