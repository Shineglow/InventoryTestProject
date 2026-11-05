using System;
using System.Collections.Generic;
using R3;

namespace InventoryCode.UI
{
	public class CellCursorHover : IDisposable
	{
		private readonly CompositeDisposable _disposable = new();
		private CellIndexMap _map;
		private CellCursor _cursor;

		public void Bind(IReadOnlyList<ItemStackView> slots, CellIndexMap map, CellCursor cursor)
		{
			_map = map;
			_cursor = cursor;
			_disposable.Clear();

			for (int i = 0; i < slots.Count; i++)
				slots[i].PointerEntered.Subscribe(OnPointerEntered).AddTo(_disposable);
		}

		public void Dispose()
		{
			_disposable.Dispose();
		}

		private void OnPointerEntered(ItemStackView view)
		{
			if (!_map.WorldToIndex(view, out int index))
				return;

			_cursor.SetIndex(index);
		}
	}
}
