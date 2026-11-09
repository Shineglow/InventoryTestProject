using System;
using InventoryCode.Data;
using R3;
using UnityEngine;

namespace InventoryCode.Gameplay
{
	public class ItemsStack
	{
		private Subject<ItemsStack> _onStackModified = new Subject<ItemsStack>();
		public ItemDefinitionData ItemId { get; private set; }

		public int MaximumCount { get; private set; }
		public int Count { get; private set; }

		public Observable<ItemsStack> OnStackModified => _onStackModified;

		public void Setup(ItemDefinitionData itemId,  int maximumCount)
		{
			ItemId = itemId;
			MaximumCount = maximumCount;
			Count = 0;
		}

		public int AddItem(int amount)
		{
			if (amount <= 0)
				return amount;
			
			int emptySpace = MaximumCount - Count;
			Count += Mathf.Min(amount, emptySpace);
			_onStackModified.OnNext(this);
			return Mathf.Max(amount - emptySpace, 0);
		}

		public int RemoveItem(int amount)
		{
			if (amount <= 0)
				return amount;
			
			int removed = Mathf.Min(Count, amount);
			Count -= removed;
			if (Count == 0)
			{
				ItemId = null;
				MaximumCount = 0;
			}
			_onStackModified.OnNext(this);
			return removed;
		}

		public bool IsEmpty()
		{
			return Count == 0;
		}
	}
}
