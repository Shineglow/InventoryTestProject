using System;
using System.Collections.Generic;
using System.Linq;
using InventoryCode.Data;
using InventoryCode.Data.ItemConfigurationComponents;
using NUnit.Framework.Internal.Commands;
using ObservableCollections;
using Unity.VisualScripting;

namespace InventoryCode.Gameplay
{
	public class Container : IGameContainer
	{
		private ObservableList<ItemsStack> _slots;

		public IReadOnlyObservableList<ItemsStack> ItemsStacks => _slots;
		public GameplayTag FilterTag { get; private set; }

		public Container(GameplayTag filterTag, int containerSize)
		{
			FilterTag = filterTag;
			_slots = MakeContainer(containerSize);
		}

		public int AddItems(ItemDefinitionData itemDefinition, int amount)
		{
			if(!IsItemFitContainerFilter(itemDefinition))
				return amount;
			
			int remainder = amount;

			while (remainder > 0)
			{
				var slot = GetSlotForItem(itemDefinition);
				if (slot == null)
					return remainder;
				if (slot.IsEmpty())
					SetupSlot(slot, itemDefinition);
				remainder = slot.AddItem(remainder);
			}
			
			return remainder;
		}

		private bool IsItemFitContainerFilter(ItemDefinitionData itemDefinition)
		{
			if(FilterTag == GameplayTag.None)
				return true;
			ItemConfigurationData configuration = ItemsConfiguration.Instance.GetItemConfigurationData(itemDefinition);
			return configuration.Tags.Contains(FilterTag);
		}

		private void SetupSlot(ItemsStack slot, ItemDefinitionData itemDefinition)
		{
			ItemConfigurationData configuration = ItemsConfiguration.Instance.GetItemConfigurationData(itemDefinition);
			StackData stackData = configuration.Components.FirstOrDefault(i => i.GetType() == typeof(StackData)) as StackData;
			if(stackData == null)
				throw new Exception("Could not find StackData");
			
			slot.Setup(itemDefinition, stackData.StackSize);
		}

		private ItemsStack GetSlotForItem(ItemDefinitionData itemDefinition)
		{
			ItemsStack emptySlot = null;

			for (int i = 0; i < _slots.Count; i++)
			{
				ItemsStack itemsStack = _slots[i];
				if (itemsStack.ItemId == itemDefinition && itemsStack.Count < itemsStack.MaximumCount)
					return itemsStack;

				if (emptySlot == null && itemsStack.IsEmpty())
					emptySlot = itemsStack;
			}

			return emptySlot;
		}

		private ObservableList<ItemsStack> MakeContainer(int containerSize)
		{
			var container = new ObservableList<ItemsStack>(containerSize);
			for (int i = 0; i < containerSize; i++)
				container.Add(new ItemsStack());
			return container;
		}
	}
}
