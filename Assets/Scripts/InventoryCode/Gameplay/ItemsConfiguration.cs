using System.Collections.Generic;
using InventoryCode.Data;

namespace InventoryCode.Gameplay
{
	public class ItemsConfiguration
	{
		public static readonly ItemsConfiguration Instance = new ItemsConfiguration();

		private ItemsConfiguration()
		{
		}

		private Dictionary<string, ItemConfigurationData> _itemConfigurations = new();

		public void AddItemConfiguration(ItemConfigurationData item)
		{
			_itemConfigurations.Add(item.Definition.EntityId, item);
		}

		public ItemConfigurationData GetItemConfigurationData(ItemDefinitionData id)
		{
			return _itemConfigurations[id.EntityId];
		}
	}
}
