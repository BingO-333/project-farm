using System;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(MovementInfo))]
    public class Player : MonoBehaviour
    {
        public MovementInfo Movement { get; private set; }
//        public Inventory Inventory { get; private set; }

        private void Awake()
        {
            Movement = GetComponent<MovementInfo>();
        }
    }
}
