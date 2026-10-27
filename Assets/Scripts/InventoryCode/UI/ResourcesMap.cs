using System.Collections.Generic;
using InventoryCode.Data;
using UnityEngine;

namespace InventoryCode.UI
{
	public class ResourcesMap
	{
		public static readonly ResourcesMap Instance = new ResourcesMap();
		
		private readonly Dictionary<ItemDefinitionData, ItemResources> _definitionToItemResources = 
			new Dictionary<ItemDefinitionData, ItemResources>();

		private ResourcesMap(){}

		public void BindAssets(ItemResources resources)
		{
			_definitionToItemResources[resources.ItemDefinition] = resources;
		}
		
		public ItemResources GetItemResources(ItemDefinitionData itemId)
		{
			if(_definitionToItemResources.TryGetValue(itemId, out ItemResources resources))
				return resources;
			Debug.LogError($"Item {itemId} does not exist.");
			return null;
		}
	}
}
