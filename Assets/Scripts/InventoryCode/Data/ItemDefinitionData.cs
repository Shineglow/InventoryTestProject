using System;
using UnityEngine;

namespace InventoryCode.Data
{
	[CreateAssetMenu(menuName = "Create ItemDefinitionData", fileName = "ItemDefinitionData", order = 0)]
	public class ItemDefinitionData : ScriptableObject
	{
		[SerializeField]
		private string _entityId;

		public string EntityId => _entityId;

		public static bool operator ==(ItemDefinitionData a, ItemDefinitionData b)
		{
			if (a == b) return true;
			if (!a || !b) return false;
			return a.EntityId == b.EntityId;
		}

		public static bool operator !=(ItemDefinitionData a, ItemDefinitionData b)
		{
			return !(a == b);
		}

		private bool Equals(ItemDefinitionData other)
		{
			return base.Equals(other) && _entityId == other._entityId;
		}

		public override bool Equals(object obj)
		{
			if (obj is null) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != GetType()) return false;
			return Equals((ItemDefinitionData)obj);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(base.GetHashCode(), _entityId);
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
