using System;
using System.Collections.Generic;
using UnityEngine;

namespace InventoryCode.Data.ItemConfigurationComponents
{
	[Serializable]
	public abstract class ItemComponentData
	{
		private GameplayTag[] _gameplayTags;
		public IReadOnlyList<GameplayTag> Tegs => _gameplayTags;
	}

	[Serializable]
	public class EnergyRestorationData : ItemComponentData
	{
		[SerializeField]
		public int EnergyAmount;
	}

	[Serializable]
	public class AlchemyIngredientData : ItemComponentData
	{
		[Range(0, 3)][SerializeField]
		public int RedRunsAmount;

		[Range(0, 3)][SerializeField]
		public int GreenRunsAmount;

		[Range(0, 3)][SerializeField]
		public int BlueRunsAmount;
	}

	[Serializable]
	public class EmbalmingData : ItemComponentData
	{
		[Range(0, 3)][SerializeField]
		public int WhiteSkullsAmount;

		[Range(0, 3)][SerializeField]
		public int RedSkullsAmount;
	}

	[Serializable]
	public class CollarData : ItemComponentData
	{
		[Range(0, 100)][SerializeField]
		public int MinimumRedSkullsAmount;

		[Range(1, 100)][SerializeField]
		public int MaximumRedSkullsAmount;
	}

	[Serializable]
	public class StackData : ItemComponentData
	{
		[Min(1)][SerializeField]
		public int StackSize;
	}

	[Serializable]
	public class PriceData : ItemComponentData
	{
		[Min(0)][SerializeField]
		public int GoldAmount;
		[Min(0)][SerializeField]
		public int SilverAmount;
		[Min(0)][SerializeField]
		public int BronzeAmount;
	}
}
