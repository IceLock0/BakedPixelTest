using _Project.Scripts.UI.Buttons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Popup
{
    public class BasePopupUIView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _id;
        [SerializeField] private TextMeshProUGUI _additionalInfo;
        [SerializeField] private TextMeshProUGUI _weight;
        [SerializeField] private TextMeshProUGUI _max;
        [SerializeField] private Image _icon;
        [SerializeField] private EventButton _closeButton;

        public void SetInfo(string id, string additionalInfo, float weight, int max, Image icon)
        {
            _id.text = id;
            _additionalInfo.text = additionalInfo;
            _weight.text = $"{weight:f}";
            _max.text = max.ToString();
            _icon.sprite = icon.sprite;
        }
        
        private void OnEnable() =>
            _closeButton.Clicked += OnCloseButtonClicked;

        private void OnDisable() =>
            _closeButton.Clicked += OnCloseButtonClicked;

        private void OnCloseButtonClicked() =>
            Destroy(gameObject);
    }
}