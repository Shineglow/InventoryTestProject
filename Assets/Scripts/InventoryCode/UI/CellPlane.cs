using InventoryCode.Gameplay;

namespace InventoryCode.UI
{
	public enum CellDirection
	{
		Up,
		Down,
		Left,
		Right
	}

	public class CellPlane
	{
		private readonly Container _container;
		private readonly int _columnCount;

		public CellPlane(Container container, int columnCount)
		{
			_container = container;
			_columnCount = columnCount;
		}

		public int Step(int index, CellDirection direction)
		{
			if (_columnCount <= 0 || !_container.ContainsIndex(index))
				return index;

			int column = index % _columnCount;
			int row = index / _columnCount;

			switch (direction)
			{
				case CellDirection.Left:
					column -= 1;
					break;
				case CellDirection.Right:
					column += 1;
					break;
				case CellDirection.Up:
					row -= 1;
					break;
				case CellDirection.Down:
					row += 1;
					break;
			}

			if (column < 0 || column >= _columnCount || row < 0)
				return index;

			int next = row * _columnCount + column;
			if (!_container.ContainsIndex(next))
				return index;

			return next;
		}

		public bool Place(int index, out int accepted)
		{
			if (!_container.ContainsIndex(index))
			{
				accepted = default;
				return false;
			}

			accepted = index;
			return true;
		}
	}
}
