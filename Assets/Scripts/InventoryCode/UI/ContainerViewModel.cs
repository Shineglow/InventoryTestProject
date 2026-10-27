using InventoryCode.Gameplay;
using ObservableCollections;

namespace InventoryCode.UI
{
	public class ContainerViewModel
	{
		private IGameContainer _container;

		private ObservableList<ItemsStackViewModel> _itemsViewModel = new ObservableList<ItemsStackViewModel>();
		public IReadOnlyObservableList<ItemsStackViewModel> ItemsViewModel;

		public void Bind(IGameContainer container)
		{
			_container = container;

			_itemsViewModel.ForEach(i => i.Dispose());
			_itemsViewModel.Clear();

			foreach (ItemsStack containerItemsStack in _container.ItemsStacks)
			{
				ItemsStackViewModel itemsStackViewModel = new ItemsStackViewModel();
				itemsStackViewModel.Bind(containerItemsStack);
				_itemsViewModel.Add(itemsStackViewModel);
			}
		}
	}
}
