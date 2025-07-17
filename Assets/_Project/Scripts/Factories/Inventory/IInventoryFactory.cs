using _Project.Scripts.Data.Inventory;
using _Project.Scripts.Inventory;

namespace _Project.Scripts.Factories.Inventory
{
    public interface IInventoryFactory
    {
        public InventoryController Create(InventoryData sourceData = null);
    }
}