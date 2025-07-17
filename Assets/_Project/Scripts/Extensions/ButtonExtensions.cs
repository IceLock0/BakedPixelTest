using UnityEngine.Events;
using UnityEngine.UI;

namespace _Project.Scripts.Extensions
{
    public static class ButtonExtensions
    {
        public static void AddListener(this Button button, UnityAction action)
        {
            button.onClick.AddListener(action);
        }
        
        public static void RemoveListener(this Button button, UnityAction action)
        {
            button.onClick.AddListener(action);
        }
    }
}