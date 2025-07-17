using System;
using _Project.Scripts.UI.Popup;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Inventory
{
    public class InventoryItemUIView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _amount;

        private InventoryItemPopUIView _itemPopupView;

        private const int MIN_AMOUNT_TO_SHOW = 1;

        public event Action<BasePopupUIView> PopupCreated; 
        
        public void SetData(Image icon, int amount)
        {
            _icon.sprite = icon.sprite;

            _amount.text = amount.ToString();
            _amount.gameObject.SetActive(amount > MIN_AMOUNT_TO_SHOW);
        }

        public void Clear()
        {
            _icon.sprite = null;
            _amount.text = "";
            _amount.gameObject.SetActive(false);
        }

        private void Awake()
        {
            _itemPopupView = GetComponent<InventoryItemPopUIView>();
            _amount.gameObject.SetActive(false);
        }

        private void OnEnable() =>
            _itemPopupView.PopupOpened += OnPopupOpened;

        private void OnDisable() =>
            _itemPopupView.PopupOpened -= OnPopupOpened;

        private void OnPopupOpened(BasePopupUIView popup) =>
            PopupCreated?.Invoke(popup);
    }
}