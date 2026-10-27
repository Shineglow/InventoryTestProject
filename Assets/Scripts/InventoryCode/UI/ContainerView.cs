using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace InventoryCode.UI
{
	public class ContainerView : MonoBehaviour
	{
		[SerializeField]
		private GridLayoutGroup gridLayoutGroup;

		[SerializeField]
		private List<ItemStackView> itemStackViews;

		public void Bind(ContainerViewModel containerViewModel)
		{
			for (int i = 0; i < containerViewModel.ItemsViewModel.Count; i++)
			{
				itemStackViews[i].Bind(containerViewModel.ItemsViewModel[i]);
			}
		}
	}

	public class ViewsPools
	{
		public IPool<T> GetPool<T>()
		{
			throw new NotImplementedException();
		}
	}

	public class Pool<T> : IPool<T>
	{
		public ReadOnlyReactiveProperty<int> Remainder { get; }
		public T Get()
		{
			throw new NotImplementedException();
		}

		public UniTask<IReadOnlyList<T>> RequestMany(int amount)
		{
			throw new NotImplementedException();
		}
	}

	public interface IPool<T>
	{
		public ReadOnlyReactiveProperty<int> Remainder { get; }
		public T Get();
		public UniTask<IReadOnlyList<T>> RequestMany(int amount);
	}
}
