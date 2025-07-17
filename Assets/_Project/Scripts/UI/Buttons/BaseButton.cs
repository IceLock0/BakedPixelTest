using _Project.Scripts.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Buttons
{
    public abstract class BaseButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        
        protected abstract void OnClick();
        
        private void Awake()
        {
            if(_button == null)
                _button = this.GetComponentInChildrenAndSelf<Button>();
        }

        private void OnEnable() =>
            _button.AddListener(OnClick);
        
        private void OnDisable() =>
            _button.RemoveListener(OnClick);
    }
}