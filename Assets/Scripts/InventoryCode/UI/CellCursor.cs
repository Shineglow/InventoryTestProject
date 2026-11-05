using R3;

namespace InventoryCode.UI
{
	public class CellCursor
	{
		private readonly Subject<int> _indexChanged = new();
		private int _index = -1;

		public int Index => _index;

		public Observable<int> IndexChanged => _indexChanged;

		public void SetIndex(int index)
		{
			if (index == _index)
				return;

			_index = index;
			_indexChanged.OnNext(index);
		}
	}
}
