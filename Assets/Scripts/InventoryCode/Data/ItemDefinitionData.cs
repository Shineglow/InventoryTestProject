using System;
using UnityEngine;

namespace InventoryCode.Data
{
	[CreateAssetMenu(menuName = "Create ItemDefinitionData", fileName = "ItemDefinitionData", order = 0)]
	public class ItemDefinitionData : ScriptableObject, IEquatable<ItemDefinitionData>
	{
		[SerializeField]
		private string _entityId;

		public string EntityId => _entityId;

		public static bool operator ==(ItemDefinitionData a, ItemDefinitionData b)
		{
			// Cast picks UnityEngine.Object.==, which sees a destroyed asset as missing
			// and does not call ItemDefinitionData.== again.
			bool aMissing = (UnityEngine.Object)a == null;
			bool bMissing = (UnityEngine.Object)b == null;
			if (aMissing || bMissing)
				return aMissing && bMissing;

			return a.EntityId == b.EntityId;
		}

		public static bool operator !=(ItemDefinitionData a, ItemDefinitionData b)
		{
			return !(a == b);
		}

		public bool Equals(ItemDefinitionData other)
		{
			return this == other;
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as ItemDefinitionData);
		}

		public override int GetHashCode()
		{
			if ((UnityEngine.Object)this == null || _entityId == null)
				return 0;

			return _entityId.GetHashCode();
		}

		#if UNITY_EDITOR
		private void OnEnable()
		{
			if (string.IsNullOrWhiteSpace(_entityId))
			{
				_entityId = Guid.NewGuid().ToString();
			}
		}

		private void OnValidate()
		{
			if (string.IsNullOrWhiteSpace(_entityId))
			{
				_entityId = Guid.NewGuid().ToString();
			}
		}
		#endif
	}
}
