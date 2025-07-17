using Component = UnityEngine.Component;

namespace _Project.Scripts.Extensions
{
    public static class ComponentExtensions
    {
        public static T GetComponentInChildrenAndSelf<T>(this Component component)
        {
            var target = component.GetComponent<T>();

            if (target == null)
                target = component.GetComponentInChildren<T>();

            return target;
        }
    }
}