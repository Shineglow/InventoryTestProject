using InventoryCode.Gameplay;
using ObservableCollections;
using R3;
using UnityEngine;

namespace InventoryCode.UI
{
	public class InventoryView : MonoBehaviour
	{
		private InventoryViewModel _viewModel;

		public void Bind(InventoryViewModel viewModel)
		{
			_viewModel = viewModel;
			_viewModel.Bags.ObserveAdd().Subscribe(UpdateBags);
		}

		private void UpdateBags(CollectionAddEvent<IGameContainer> foo)
		{
			// foo.Value.ItemsStacks;
		}
	}
}
