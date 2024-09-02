using System.Collections;
using UnityEngine;

namespace Game
{
	public abstract class InteractableZone : MonoBehaviour
	{
		public Transform TriggerTransform => TriggerZone.transform;

		[field: SerializeField] protected SeparateTrigger TriggerZone { get; private set; }

		[SerializeField] bool _interactWhenStop = true;

		private Coroutine _interactCoroutine;

		protected virtual void Awake()
		{
			TriggerZone.TriggerEnter += TriggerEnter;
			TriggerZone.TriggerExit += TriggerExit;
		}

		private void TriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out Player player))
            {
				if (_interactWhenStop)
				{
					if (_interactCoroutine != null)
						StopCoroutine(_interactCoroutine);
					_interactCoroutine = StartCoroutine(WaitPlayerStop(player));
				}
				else
					StartInteract(player);
            }
		}

		private void TriggerExit(Collider other)
		{
			if (other.TryGetComponent(out Player player))
            {
				if (_interactCoroutine != null)
					StopCoroutine(_interactCoroutine);
				StopInteract(player);
			}
		}

		protected abstract void StartInteract(Player player);
		protected abstract void StopInteract(Player player);

		private IEnumerator WaitPlayerStop(Player player)
        {
			yield return new WaitUntil(() => player.Movement.IsMoving == false);
			StartInteract(player);
		}
	}
}