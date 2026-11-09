using System.Collections.Generic;
using System.Linq;
using InventoryCode.Data;
using ObservableCollections;

namespace InventoryCode.Gameplay
{
	public class Inventory : IAddItem
	{
		private ObservableList<IGameContainer> _bags;
		public IReadOnlyObservableList<IGameContainer> Bags => _bags;

		public Inventory()
		{
			_bags = new ObservableList<IGameContainer>();
			_bags.Add(new Container(GameplayTag.None, 25));
		}

		public int AddItems(ItemDefinitionData itemDefinition, int amount)
		{
			var itemConfiguration = ItemsConfiguration.Instance.GetItemConfigurationData(itemDefinition);
			var itemTags = itemConfiguration.Tags;
			
			int remainder = amount;

			while (remainder > 0)
			{
				ItemsStack stack = GetMostSuitableSlot(itemDefinition, itemTags);
				if (stack == null)
					return remainder;
				remainder = stack.AddItem(remainder);
			}
			return remainder;
		}

		private ItemsStack GetMostSuitableSlot(ItemDefinitionData itemDefinition, GameplayTag[] itemTags)
		{
			GameplayTag gameplayTag = GameplayTag.None;
			ItemsStack stack = null;
			bool validStackWasFound = false;
			
			for (int i = 0; i < _bags.Count; i++)
			{
				var actualContainer = _bags[i];
				IReadOnlyList<ItemsStack> stacks = actualContainer.ItemsStacks;
				for (int j = 0; j < stacks.Count; j++)
				{
					ItemsStack actualStack = stacks[j];
					if (actualStack.ItemId == itemDefinition && actualStack.Count < stacks[i].MaximumCount)
					{
						stack = actualStack;
						validStackWasFound = true;
						break;
					}

					if (stack == null)
					{
						if (actualStack.Count == 0)
						{
							if (actualContainer.FilterTag == GameplayTag.None || itemTags.Contains(actualContainer.FilterTag))
							{
								stack = actualStack;
								gameplayTag = actualContainer.FilterTag;
							}
						}
					}
					else if(gameplayTag == GameplayTag.None)
					{
						if (actualStack.Count == 0)
						{
							if (itemTags.Contains(actualContainer.FilterTag))
							{
								stack = actualStack;
								gameplayTag = actualContainer.FilterTag;
							}
						}
					}
				}
				
				if (validStackWasFound)
					break;
			}
			
			return stack;
		}
	}
}
