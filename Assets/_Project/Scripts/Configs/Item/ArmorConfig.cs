using UnityEngine;

namespace _Project.Scripts.Configs.Item
{
    [CreateAssetMenu(fileName = "ArmorConfig", menuName = "Configs/Inventory/Items/Armor", order = 0)]
    public class ArmorConfig : ItemConfig
    {
        [field: SerializeField] public int Value { get; private set; }
    }
}