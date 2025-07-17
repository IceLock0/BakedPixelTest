using System;
using _Project.Scripts.UI.Buttons;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Popup
{
    public class InventoryItemPopUIView : MonoBehaviour
    {
        [SerializeField] private BasePopupUIView _prefab;
        [SerializeField] private Image _icon;
        [SerializeField] private EventButton _popupButton;

        public event Action<BasePopupUIView> PopupOpened;
        
        private void OnEnable() =>
            _popupButton.Clicked += OnPopupButtonClicked;

        private void OnDisable() =>
            _popupButton.Clicked += OnPopupButtonClicked;
        
        private void OnPopupButtonClicked()
        {
            if(_icon.sprite != null)
            {
                var popup = Instantiate(_prefab, transform.root);
                PopupOpened?.Invoke(popup);
            }
        }
    }
}