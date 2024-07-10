using UnityEngine;

namespace Game
{
	[CreateAssetMenu(menuName = "Data/ItemData", fileName = "DefaultItemData")]
	public class ItemData : ScriptableObject
	{
		public string Name => name;

		[field: SerializeField] public int Cost { get; private set; } = 10;

		[field: SerializeField] public Sprite Icon { get; private set; }

		[Space] [SerializeField] ItemModel _modelPrefab;

		public ItemModel SpawnItemModel(Vector3 position, Quaternion rotation, Transform parent)
        {
			ItemModel spawnedModel = Instantiate(_modelPrefab, position, rotation, parent);
			spawnedModel.Setup(this);

			return spawnedModel;
        }
	}
}

