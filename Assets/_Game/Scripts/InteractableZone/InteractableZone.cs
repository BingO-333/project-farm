using UnityEngine;

namespace Game
{
	public abstract class InteractableZone : MonoBehaviour
	{
		public Transform TriggerTransform => TriggerZone.transform;

		[field: SerializeField] protected SeparateTrigger TriggerZone { get; private set; }

		protected virtual void Awake()
		{
			TriggerZone.TriggerEnter += TriggerEnter;
			TriggerZone.TriggerExit += TriggerExit;
		}

		private void TriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out Player _player))
				StartInteract(_player);
		}

		private void TriggerExit(Collider other)
		{
			if (other.TryGetComponent(out Player _player))
				StopInteract(_player);
		}

		protected abstract void StartInteract(Player player);
		protected abstract void StopInteract(Player player);
	}
}