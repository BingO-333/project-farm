using System;
using System.Linq;
using UnityEngine;

namespace Game
{
	[Serializable] public class UpgradeLevel
	{
		public event Action OnLevelUp
        {
            add
            {
                foreach (var uz in _upgradesZones)
                    uz.OnMoneyTaken += value;
            }
            remove
            {
                foreach (var uz in _upgradesZones)
                    uz.OnMoneyTaken -= value;
            }
        }

		public int Level => _upgradesZones == null || _upgradesZones.Length == 0 ? 0 : _upgradesZones.Count(z => z.AllMoneyTaken);

		[SerializeField] UpgradeZone[] _upgradesZones;
	}

}