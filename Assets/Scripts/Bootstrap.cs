using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using InventoryCode.Data;
using InventoryCode.Data.ItemConfigurationComponents;
using InventoryCode.Gameplay;
using InventoryCode.UI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityPipeline.Microsoft.CodeAnalysis.CSharp.Syntax;

public class Bootstrap : MonoBehaviour
{
	[SerializeField]
	private ItemStackView _itemStackViewLeft;

	[SerializeField]
	private ItemStackView _itemStackViewRight;

	[SerializeField]
	private ItemDefinitionData _itemDefinitionLeft;

	[SerializeField]
	private ItemDefinitionData _itemDefinitionRight;


	private void Start()
	{
		InitAsync().Forget();
	}

	private async UniTask InitAsync()
	{
		bool itemsWasAdded = await TryAddItemsDataToConfiguration();

		if (!itemsWasAdded)
			return;

		bool itemResourcesWasAdded = await TryAddItemsDataToResources();
		
		if (!itemResourcesWasAdded)
			return;

		_itemStackViewLeft.Bind(ItemsStackViewModel(_itemDefinitionLeft));
		_itemStackViewRight.Bind(ItemsStackViewModel(_itemDefinitionRight));

		ItemsStackViewModel ItemsStackViewModel(ItemDefinitionData itemDefinition)
		{
			ItemsStack stack = new ItemsStack();
			if (itemDefinition)
			{
				ItemConfigurationData configuration =
					ItemsConfiguration.Instance.GetItemConfigurationData(itemDefinition);
				StackData stackData = (StackData)configuration.Components.First(i => i.GetType() == typeof(StackData));
				stack.Setup(itemDefinition, stackData.StackSize);
				stack.AddItem(stackData.StackSize);
			}

			ItemsStackViewModel itemsStackViewModel = new ItemsStackViewModel();
			itemsStackViewModel.Bind(stack);
			return itemsStackViewModel;
		}
	}

	private async UniTask<bool> TryAddItemsDataToConfiguration()
	{
		AsyncOperationHandle<IList<ItemConfigurationData>> handle =
			Addressables.LoadAssetsAsync<ItemConfigurationData>("ItemConfigData", true);

		await handle.ToUniTask();

		if (handle.Status == AsyncOperationStatus.Succeeded)
		{
			foreach (ItemConfigurationData itemConfigurationData in handle.Result)
			{
				ItemsConfiguration.Instance.AddItemConfiguration(itemConfigurationData);
			}

			return true;
		}

		Debug.LogError(handle.Status.ToString());
		return false;
	}
	
	private async UniTask<bool> TryAddItemsDataToResources()
	{
		AsyncOperationHandle<IList<ItemResources>> handle =
			Addressables.LoadAssetsAsync<ItemResources>("ItemResources", true);

		await handle.ToUniTask();

		if (handle.Status == AsyncOperationStatus.Succeeded)
		{
			foreach (ItemResources itemConfigurationData in handle.Result)
			{
				ResourcesMap.Instance.BindAssets(itemConfigurationData);
			}

			return true;
		}

		Debug.LogError(handle.Status.ToString());
		return false;
	}
}
