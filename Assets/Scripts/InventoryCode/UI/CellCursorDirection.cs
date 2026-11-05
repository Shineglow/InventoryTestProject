using System;
using UnityEngine.InputSystem;

namespace InventoryCode.UI
{
	public class CellCursorDirection : IDisposable
	{
		private readonly CellPlane _plane;
		private readonly CellCursor _cursor;
		private readonly InputActionMap _actions;

		public CellCursorDirection(CellPlane plane, CellCursor cursor, InputActionAsset actions)
		{
			if (actions == null)
				throw new ArgumentNullException(nameof(actions));

			_plane = plane;
			_cursor = cursor;
			_actions = actions.FindActionMap("CellCursor", true);
			foreach (CellDirection direction in Enum.GetValues(typeof(CellDirection)))
				_actions.FindAction(direction.ToString(), true).performed += OnPerformed;

			_actions.Enable();
		}

		public void Dispose()
		{
			foreach (CellDirection direction in Enum.GetValues(typeof(CellDirection)))
			{
				InputAction action = _actions.FindAction(direction.ToString(), false);
				if (action != null)
					action.performed -= OnPerformed;
			}

			_actions.Disable();
		}

		private void OnPerformed(InputAction.CallbackContext context)
		{
			if (!Enum.TryParse(context.action.name, out CellDirection direction))
				return;

			int next = _plane.Step(_cursor.Index, direction);
			_cursor.SetIndex(next);
		}
	}
}
