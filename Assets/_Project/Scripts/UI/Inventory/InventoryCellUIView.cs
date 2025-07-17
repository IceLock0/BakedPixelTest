using System;
using _Project.Scripts.UI.Buttons;
using _Project.Scripts.UI.Popup;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Inventory
{
    public class InventoryCellUIView : MonoBehaviour
    {
        [SerializeField] private Image _block;

        private InventoryItemUIView _inventoryItemUIView;

        private EventButton _unblockButton;

        public event Action<InventoryCellUIView> UnblockClicked;
        public event Action<InventoryCellUIView, BasePopupUIView> PopupCreated;
        
        public void SetItem(Image icon, int amount) =>
            _inventoryItemUIView.SetData(icon, amount);

        public void CleanItem() =>
            _inventoryItemUIView.Clear();

        public void SetAvailable(bool isAvailable) =>
            _block.gameObject.SetActive(!isAvailable);

        private void Awake()
        {
            _inventoryItemUIView = GetComponentInChildren<InventoryItemUIView>();
            _unblockButton = GetComponent<EventButton>();
        }

        private void OnEnable()
        {
            _unblockButton.Clicked += OnUnblockButtonClicked;
            _inventoryItemUIView.PopupCreated += OnPopupCreated;
        }

        private void OnDisable()
        {
            _unblockButton.Clicked -= OnUnblockButtonClicked;
            _inventoryItemUIView.PopupCreated -= OnPopupCreated;
        }

        private void OnUnblockButtonClicked() =>
            UnblockClicked?.Invoke(this);
        
        private void OnPopupCreated(BasePopupUIView popup) =>
            PopupCreated?.Invoke(this, popup);
    }
}