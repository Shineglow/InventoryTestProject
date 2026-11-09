using System.Collections.Generic;
using InventoryCode.Data;
using ObservableCollections;

namespace InventoryCode.Gameplay
{
	public interface IGameContainer : IAddItem
	{
		IReadOnlyObservableList<ItemsStack> ItemsStacks { get; }
		GameplayTag FilterTag { get; }
	}

	public interface IAddItem
	{
		int AddItems(ItemDefinitionData itemDefinition, int amount);
	}
}
