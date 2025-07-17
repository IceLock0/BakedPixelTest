using UnityEngine;

namespace _Project.Scripts.Configs.Inventory
{
    [CreateAssetMenu(fileName = "InventoryConfig", menuName = "Configs/Inventory/Inventory", order = -90)]

    public class InventoryConfig : ScriptableObject
    {
        [field: SerializeField] public string Id { get; private set; }
        [field: SerializeField] public int Size { get; private set; }
        [field: SerializeField] public int OpenedCells { get; private set; }
        [field: SerializeField] public int CellPrice { get; private set; }
    }
}