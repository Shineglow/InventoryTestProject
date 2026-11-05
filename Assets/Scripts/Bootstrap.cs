using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using InventoryCode;
using InventoryCode.Data;
using InventoryCode.Gameplay;
using InventoryCode.UI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Bootstrap : MonoBehaviour
{
	private const int SlotCount = 12;
	private const int ColumnCount = 4;
	private const string ContainerAddress = "Container";
	private const string CellIndexMarkAddress = "CellIndexMark";

	[SerializeField]
	private Transform _containersList;

	private CellCursor _cursor;
	private CellCursorViewModel _cursorViewModel;
	private CellCursorView _cursorView;
	private CellCursorHover _hover;

	private void Start()
	{
		InitAsync().Forget();
	}

	private async UniTask InitAsync()
	{
		IList<ItemConfigurationData> configurations = await LoadConfigurations();
		if (configurations == null)
			return;

		bool itemResourcesWasAdded = await TryAddItemsDataToResources();
		if (!itemResourcesWasAdded)
			return;

		GameObject cursorViewPrefab = await LoadCursorViewPrefab();
		if (cursorViewPrefab == null)
			return;

		Container container = new Container(GameplayTag.None, SlotCount);
		GameObject prefab = await Addressables.LoadAssetAsync<GameObject>(ContainerAddress);
		GameObject instance = Instantiate(prefab, _containersList);
		ContainerView view = instance.GetComponent<ContainerView>();
		view.SetColumnCount(ColumnCount);

		ContainerViewModel viewModel = new ContainerViewModel();
		viewModel.Bind(container);
		view.Bind(viewModel);

		CellIndexMap map = new CellIndexMap();
		map.Reset();
		IReadOnlyList<ItemStackView> slots = view.SlotViews;
		for (int i = 0; i < slots.Count; i++)
			map.BindWorldWithIndex(slots[i], i);

		_cursor = new CellCursor();
		_cursorViewModel = new CellCursorViewModel();
		_cursorViewModel.Bind(_cursor);
		_cursorView = new CellCursorView(map, cursorViewPrefab);
		_cursorView.Bind(_cursorViewModel);
		_hover = new CellCursorHover();
		_hover.Bind(slots, map, _cursor);
		_cursor.SetIndex(0);

		AddExistingItem(container, configurations, "Flour", 5);
		AddExistingItem(container, configurations, "Alkali", 3);
		AddExistingItem(container, configurations, "Collar", 1);
	}

	private void AddExistingItem(
		Container container,
		IList<ItemConfigurationData> configurations,
		string itemName,
		int amount)
	{
		ItemConfigurationData configuration = configurations.First(item => item.name == itemName);
		container.AddItems(configuration.Definition, amount);
	}

	private async UniTask<GameObject> LoadCursorViewPrefab()
	{
		AsyncOperationHandle<GameObject> handle =
			Addressables.LoadAssetAsync<GameObject>(CellIndexMarkAddress);

		await handle.ToUniTask();

		if (handle.Status != AsyncOperationStatus.Succeeded)
		{
			Debug.LogError(handle.Status.ToString());
			return null;
		}

		return handle.Result;
	}

	private async UniTask<IList<ItemConfigurationData>> LoadConfigurations()
	{
		AsyncOperationHandle<IList<ItemConfigurationData>> handle =
			Addressables.LoadAssetsAsync<ItemConfigurationData>("ItemConfigData", null);

		await handle.ToUniTask();

		if (handle.Status != AsyncOperationStatus.Succeeded)
		{
			Debug.LogError(handle.Status.ToString());
			return null;
		}

		foreach (ItemConfigurationData itemConfigurationData in handle.Result)
			ItemsConfiguration.Instance.AddItemConfiguration(itemConfigurationData);

		return handle.Result;
	}

	private async UniTask<bool> TryAddItemsDataToResources()
	{
		AsyncOperationHandle<IList<ItemResources>> handle =
			Addressables.LoadAssetsAsync<ItemResources>("ItemResources", null);

		await handle.ToUniTask();

		if (handle.Status == AsyncOperationStatus.Succeeded)
		{
			foreach (ItemResources itemResources in handle.Result)
				ResourcesMap.Instance.BindAssets(itemResources);

			return true;
		}

		Debug.LogError(handle.Status.ToString());
		return false;
	}
}
