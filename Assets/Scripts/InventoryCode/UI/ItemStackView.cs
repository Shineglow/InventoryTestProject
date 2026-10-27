using System;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InventoryCode.UI
{
	public class ItemStackView : MonoBehaviour, IDisposable
	{
		[SerializeField]
		private Image icon;
		
		[SerializeField]
		private TextMeshProUGUI itemsCount;

		private ItemsStackViewModel _stackViewModel;
		private CompositeDisposable _disposable = new CompositeDisposable();

		public void Bind(ItemsStackViewModel stackViewModel)
		{
			_stackViewModel = stackViewModel;

			_stackViewModel.OnImageChanged.Subscribe(ChangeImage).AddTo(_disposable);
			_stackViewModel.Count.Subscribe(ChangeCount).AddTo(_disposable);
		}

		private void ChangeImage(Sprite newSprite)
		{
			icon.sprite = newSprite;
		}
		
		private void ChangeCount(string newSprite)
		{
			itemsCount.text = newSprite;
		}

		public void Dispose()
		{
			_disposable.Clear();
			icon.sprite = null;
			itemsCount.text = "";
			_stackViewModel = null;
		}
	}
}
