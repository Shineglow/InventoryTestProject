using System.Collections.Generic;
using UnityEngine;

namespace InventoryCode.UI
{
	public class CellIndexMap
	{
		private readonly Dictionary<ItemStackView, int> _viewToIndex = new();
		private readonly Dictionary<int, ItemStackView> _indexToView = new();

		public void Reset()
		{
			_viewToIndex.Clear();
			_indexToView.Clear();
		}

		public void BindWorldWithIndex(ItemStackView view, int index)
		{
			_viewToIndex.Add(view, index);
			_indexToView.Add(index, view);
		}

		public bool WorldToIndex(ItemStackView view, out int index)
		{
			return _viewToIndex.TryGetValue(view, out index);
		}

		public bool IndexToWorld(int index, out Vector3 worldPosition)
		{
			if (!TryGetView(index, out ItemStackView view))
			{
				worldPosition = default;
				return false;
			}

			worldPosition = view.transform.position;
			return true;
		}

		public bool TryGetView(int index, out ItemStackView view)
		{
			return _indexToView.TryGetValue(index, out view);
		}
	}
}
