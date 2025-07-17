using System;
using _Project.Scripts.Configs.Item;
using _Project.Scripts.Data.Inventory;
using _Project.Scripts.Services.Log;

namespace _Project.Scripts.Inventory.Commands
{
    public class RemoveItemInventoryCommand : BaseInventoryCommand
    {
        public RemoveItemInventoryCommand(ILogService logService, InventoryData inventoryData, ItemConfig[] itemConfigs) 
            : base(logService, inventoryData, itemConfigs)
        {
        }
        
        public override void Execute(string itemId, int amount)
        {
            LogService.Log($"Try to REMOVE item with id: {itemId}, amount: {amount}");
            
            var itemConfig = GetItemConfig(itemId);
            if (itemConfig == null)
                return;

            var remaining = Remove(itemConfig, amount);
            
            if(remaining > 0)
                LogService.Warning($"Not enough items to remove. Remaining: {remaining}");
        }

        private int Remove(ItemConfig config, int amount)
        {
            var remaining = amount;
            for (var i = 0; i < InventoryData.Cells.Length; i++)
            {
                var cell = InventoryData.Cells[i];
                if(!cell.IsAvailable)
                    continue;
                
                var item = cell.Item;

                if (item != null && item.Id == config.Id)
                {
                    var remove = Math.Min(item.Amount, remaining);
                    item.Amount -= remove;
                    remaining -= remove;

                    LogService.Log($"Removed from slot: {i + 1}, remaining: {remaining}");

                    if (item.Amount <= 0)
                        cell.Item = null;

                    if (remaining <= 0)
                        break;
                }
            }

            return remaining;
        }
    }
}