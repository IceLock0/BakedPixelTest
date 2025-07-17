using System;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI.Buttons.View
{
    public class WalletButtonUIView : MonoBehaviour
    {
        [SerializeField] private EventButton _addCoinsButton;
        [SerializeField] private TextMeshProUGUI _coins;

        public event Action Clicked;
        
        public void SetCoins(int coins)
            => _coins.text = coins.ToString();

        private void OnEnable() =>
            _addCoinsButton.Clicked += OnAddCoinsButtonClicked;

        private void OnDisable() =>
            _addCoinsButton.Clicked -= OnAddCoinsButtonClicked;
        
        private void OnAddCoinsButtonClicked() =>
            Clicked?.Invoke();
    }
}