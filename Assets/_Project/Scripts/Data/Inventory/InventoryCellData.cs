using System;

namespace _Project.Scripts.Data.Inventory
{
    [Serializable]
    public class InventoryCellData
    {
        public InventoryItemData Item;
        public bool IsAvailable;
    }
}