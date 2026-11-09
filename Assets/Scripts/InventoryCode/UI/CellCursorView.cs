using System;
using R3;
using UnityEngine;

namespace InventoryCode.UI
{
	public class CellCursorView : IDisposable
	{
		private readonly CellIndexMap _map;
		private readonly RectTransform _mark;
		private readonly CompositeDisposable _disposable = new();
		private int _index = -1;
		private bool _placed;

		public CellCursorView(CellIndexMap map, GameObject prefab)
		{
			_map = map;
			_mark = UnityEngine.Object.Instantiate(prefab).GetComponent<RectTransform>();
		}

		public void Bind(CellCursorViewModel viewModel)
		{
			viewModel.Index.Subscribe(Show).AddTo(_disposable);
		}

		public void Dispose()
		{
			_disposable.Dispose();
		}

		private void Show(int index)
		{
			if (_placed && index == _index)
				return;

			if (!_map.TryGetView(index, out ItemStackView slot))
				return;

			_index = index;
			_placed = true;
			_mark.SetParent(slot.transform, false);
			_mark.anchorMin = Vector2.zero;
			_mark.anchorMax = Vector2.one;
			_mark.offsetMin = Vector2.zero;
			_mark.offsetMax = Vector2.zero;
			_mark.localScale = Vector3.one;
			_mark.SetAsLastSibling();
		}
	}
}
