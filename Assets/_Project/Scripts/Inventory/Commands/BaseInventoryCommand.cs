using System.Linq;
using _Project.Scripts.Configs.Item;
using _Project.Scripts.Data.Inventory;
using _Project.Scripts.Services.Log;

namespace _Project.Scripts.Inventory.Commands
{
    public abstract class BaseInventoryCommand : IInventoryCommand
    {
        protected readonly ILogService LogService;
        protected readonly InventoryData InventoryData;
        protected readonly ItemConfig[] ItemConfigs;
        
        protected BaseInventoryCommand(ILogService logService, InventoryData inventoryData, ItemConfig[] itemConfigs)
        {
            LogService = logService;
            InventoryData = inventoryData;
            ItemConfigs = itemConfigs;
        }

        public abstract void Execute(string itemId, int amount);
        
        protected ItemConfig GetItemConfig(string itemId)
        {
            var config = ItemConfigs.FirstOrDefault(item => item.Id == itemId);
            if(config ==  null)
                LogService.Error($"Could not find item config for id: {itemId}");

            return config;
        }
    }
}