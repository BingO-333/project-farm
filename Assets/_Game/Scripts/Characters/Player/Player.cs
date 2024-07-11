using DG.Tweening;
using System;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(MovementInfo))]
    public class Player : MonoBehaviour
    {
        public MovementInfo Movement { get; private set; }
        public Inventory Inventory { get; private set; }

        [SerializeField] SeparateTrigger _inventoryTrigger;

        private void Awake()
        {
            Movement = GetComponent<MovementInfo>();
            Inventory = GetComponent<Inventory>();

            _inventoryTrigger.TriggerEnter += OnInventoryTriggerEnter;
        }

        private void OnInventoryTriggerEnter(Collider collider)
        {
            if (collider.TryGetComponent(out ItemModel itemModel))
            {
                itemModel.DisableCollider();

                Inventory.AddItem(itemModel.Data);

                itemModel.transform.parent = transform;
                itemModel.LocalJumpTo(Vector3.up, 0.35f, 1.5f)
                    .OnComplete(() => Destroy(itemModel.gameObject));
            }
        }
    }
}
