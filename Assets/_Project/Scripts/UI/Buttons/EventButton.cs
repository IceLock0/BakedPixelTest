using System;

namespace _Project.Scripts.UI.Buttons
{
    public class EventButton : BaseButton
    {
        public event Action Clicked;
        
        protected override void OnClick()
        {
            Clicked?.Invoke();
        }
    }
}