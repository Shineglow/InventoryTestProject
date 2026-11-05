using System;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace InventoryCode.UI
{
	public class ItemStackView : MonoBehaviour, IDisposable, IPointerEnterHandler
	{
		[SerializeField]
		private Image icon;
		
		[SerializeField]
		private TextMeshProUGUI itemsCount;

		private readonly Subject<ItemStackView> _pointerEntered = new();
		private ItemsStackViewModel _stackViewModel;
		private CompositeDisposable _disposable = new CompositeDisposable();

		public Observable<ItemStackView> PointerEntered => _pointerEntered;

		public void Bind(ItemsStackViewModel stackViewModel)
		{
			_stackViewModel = stackViewModel;

			_stackViewModel.OnImageChanged.Subscribe(ChangeImage).AddTo(_disposable);
			_stackViewModel.Count.Subscribe(ChangeCount).AddTo(_disposable);
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			_pointerEntered.OnNext(this);
		}

		private void ChangeImage(Sprite newSprite)
		{
			icon.sprite = newSprite;
			icon.enabled = newSprite != null;
		}
		
		private void ChangeCount(string newSprite)
		{
			itemsCount.text = newSprite;
		}

		public void Dispose()
		{
			_disposable.Clear();
			_pointerEntered.Dispose();
			icon.sprite = null;
			itemsCount.text = "";
			_stackViewModel = null;
		}
	}
}
