using System;
using UnityEngine;

namespace Game 
{
	[RequireComponent(typeof(Collider))]
	public class SeparateTrigger : MonoBehaviour
	{
		public Action<Collider> TriggerEnter;
		public Action<Collider> TriggerExit;
    
		private void OnTriggerEnter(Collider other)
		{
			TriggerEnter?.Invoke(other);
		}

		private void OnTriggerExit(Collider other)
		{
			TriggerExit?.Invoke(other);
		}
	}    
}