using System;
using InventoryCode.Gameplay;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace InventoryCode.UI
{
	public class ItemsStackViewModel : IDisposable
	{
		private ItemsStack _stack;
		
		private ReactiveProperty<Sprite> _image;
		private ReactiveProperty<string> _count;
		
		public Observable<Sprite> OnImageChanged => _image;
		public Observable<string> Count => _count;
		
		public void Bind(ItemsStack stack)
		{
			_stack = stack;
			Dispose();
			_image = new ReactiveProperty<Sprite>();
			_count = new ReactiveProperty<string>();
			stack.OnStackModified.Subscribe(SetupData);
			SetupData(stack);
		}

		public void SetupData(ItemsStack stack)
		{
			if (stack.IsEmpty() || stack.ItemId == null)
			{
				_image.Value = null;
				_count.Value = string.Empty;
				return;
			}

			var resources = ResourcesMap.Instance.GetItemResources(stack.ItemId);
			_image.Value = resources.ItemIcon;
			_count.Value = stack.Count.ToString();
		}

		public void Dispose()
		{
			_image?.Dispose();
			_count?.Dispose();
		}
	}
}
