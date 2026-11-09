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
		private List<ItemStackView> itemStackViews = new();

		public int ColumnCount => gridLayoutGroup.constraintCount;

		private void Awake()
		{
			gridLayoutGroup = GetComponent<GridLayoutGroup>();

			if (itemStackViews.Count == 0)
				itemStackViews.AddRange(GetComponentsInChildren<ItemStackView>(true));
		}

		public void SetColumnCount(int columnCount)
		{
			gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
			gridLayoutGroup.constraintCount = columnCount;
		}

		public IReadOnlyList<ItemStackView> SlotViews => itemStackViews;

		public void Bind(ContainerViewModel containerViewModel)
		{
			EnsureSlotViews(containerViewModel.ItemsViewModel.Count);

			for (int i = 0; i < containerViewModel.ItemsViewModel.Count; i++)
				itemStackViews[i].Bind(containerViewModel.ItemsViewModel[i]);
		}

		private void EnsureSlotViews(int count)
		{
			ItemStackView template = itemStackViews[0];
			Transform parent = template.transform.parent;

			while (itemStackViews.Count < count)
			{
				ItemStackView copy = Instantiate(template, parent);
				copy.name = template.name;
				itemStackViews.Add(copy);
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
