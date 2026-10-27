using System.Collections.Generic;
using InventoryCode.Data.ItemConfigurationComponents;
using UnityEngine;

namespace InventoryCode.Data
{
	[CreateAssetMenu(menuName = "Create ItemConfigurationData", fileName = "ItemConfigurationData", order = 0)]
	public class ItemConfigurationData : ScriptableObject
	{
		[field: SerializeField]
		public ItemDefinitionData Definition { get; private set; }
		[field: SerializeField]
		public string Name { get; private set; }
		[field: SerializeField]
		public string Description { get; private set; }
		
		[field: SerializeField]
		public GameplayTag[] Tags { get; private set; }
		
		[SerializeReference]
		private List<ItemComponentData> _components = new();
		
		public IReadOnlyList<ItemComponentData> Components => _components;
	}
}
