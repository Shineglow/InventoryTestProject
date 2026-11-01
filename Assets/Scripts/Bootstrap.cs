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

	[SerializeField]
	private Transform _containersList;

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

		Container container = new Container(GameplayTag.None, SlotCount);
		GameObject prefab = await Addressables.LoadAssetAsync<GameObject>(ContainerAddress);
		GameObject instance = Instantiate(prefab, _containersList);
		ContainerView view = instance.GetComponent<ContainerView>();
		view.SetColumnCount(ColumnCount);

		ContainerViewModel viewModel = new ContainerViewModel();
		viewModel.Bind(container);
		view.Bind(viewModel);

		AddExistingItem(container, configurations, "Flour", 5);
		AddExistingItem(container, configurations, "Alkali", 3);
		AddExistingItem(container, configurations, "Collar", 1);
	}

	private static void AddExistingItem(
		Container container,
		IList<ItemConfigurationData> configurations,
		string itemName,
		int amount)
	{
		ItemConfigurationData configuration = configurations.First(item => item.name == itemName);
		container.AddItems(configuration.Definition, amount);
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
