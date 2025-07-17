using UnityEngine;

namespace _Project.Scripts.Configs.Item
{
    [CreateAssetMenu(fileName = "WeaponConfig", menuName = "Configs/Inventory/Items/Weapon", order = 0)]
    public class WeaponConfig : ItemConfig
    {
        [field: SerializeField] public AmmoConfig Ammo { get; set; }
        [field: SerializeField] public int Damage { get; set; }
    }
}