using System;
using UnityEngine;

namespace _Project.Scripts.UI.Buttons.View
{
    public abstract class BaseButtonUIView : MonoBehaviour
    {
        private EventButton _eventButton;

        public event Action Clicked;

        private void Awake() =>
            _eventButton = GetComponent<EventButton>();

        private void OnEnable() =>
            _eventButton.Clicked += OnClicked;

        private void OnDisable() =>
            _eventButton.Clicked -= OnClicked;

        private void OnClicked() =>
            Clicked?.Invoke();
    }
}