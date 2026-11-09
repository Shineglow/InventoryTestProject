using InventoryCode.Data;
using UnityEngine;
using UnityEngine.UI;

namespace InventoryCode.UI
{
	[CreateAssetMenu(menuName = "Create ItemResources", fileName = "ItemResources", order = 0)]
	public class ItemResources : ScriptableObject
	{
		[field: SerializeField]
		public ItemDefinitionData ItemDefinition { get; private set; }

		[field: SerializeField]
		public Sprite ItemIcon { get; private set; }
	}
}
