using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Configs.Item
{
    public abstract class ItemConfig : ScriptableObject
    {
        [field: SerializeField] public string Id;
        [field: SerializeField] public Image Icon;
        [field: SerializeField] public int Max { get; private set; }
        [field: SerializeField] public float Weight { get; private set; }
    }
}