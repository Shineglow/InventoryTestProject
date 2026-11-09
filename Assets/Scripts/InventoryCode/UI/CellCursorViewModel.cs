using System;
using R3;

namespace InventoryCode.UI
{
	public class CellCursorViewModel : IDisposable
	{
		private readonly ReactiveProperty<int> _index = new(-1);
		private readonly CompositeDisposable _disposable = new();

		public Observable<int> Index => _index;

		public void Bind(CellCursor cursor)
		{
			_disposable.Clear();
			cursor.IndexChanged.Subscribe(SetIndex).AddTo(_disposable);
			_index.Value = cursor.Index;
		}

		public void Dispose()
		{
			_disposable.Dispose();
			_index.Dispose();
		}

		private void SetIndex(int index)
		{
			_index.Value = index;
		}
	}
}
