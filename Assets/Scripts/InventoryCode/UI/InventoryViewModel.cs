using InventoryCode.Gameplay;
using ObservableCollections;

namespace InventoryCode.UI
{
	public class InventoryViewModel
	{
		private Inventory _inventory;
		
		public IReadOnlyObservableList<IGameContainer> Bags => _inventory.Bags;
	}
}
