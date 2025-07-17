using System;
using _Project.Scripts.Configs.Item;
using _Project.Scripts.Data.Inventory;
using _Project.Scripts.Services.Log;

namespace _Project.Scripts.Inventory.Commands
{
    public class AddItemInventoryCommand : BaseInventoryCommand
    {
        public AddItemInventoryCommand(ILogService logService, InventoryData inventoryData, ItemConfig[] itemConfigs)
            : base(logService, inventoryData, itemConfigs)
        {
        }

        public override void Execute(string itemId, int amount)
        {
            LogService.Log($"Try to ADD item with id: {itemId}, amount: {amount}");

            var itemConfig = GetItemConfig(itemId);
            if (itemConfig == null)
                return;

            var remaining = AddToExisting(itemConfig, amount);

            AddToFree(itemId, remaining);
        }

        private int AddToExisting(ItemConfig config, int amount)
        {
            var remaining = amount;
            for (var i = 0; i < InventoryData.Cells.Length; i++)
            {
                var cell = InventoryData.Cells[i];
                if(!cell.IsAvailable)
                    continue;
                
                var item = cell.Item;

                if (item != null && item.Id == config.Id && item.Amount < config.Max)
                {
                    var free = config.Max - item.Amount;
                    var add = Math.Min(free, remaining);

                    item.Amount += add;
                    remaining -= add;

                    LogService.Log($"Added to slot: {i + 1}, amount: {add}, remaining: {remaining}");

                    if (remaining <= 0)
                        break;
                }
            }

            return remaining;
        }

        private void AddToFree(string itemId, int amount)
        {
            if (amount <= 0)
                return;

            for (var i = 0; i < InventoryData.Cells.Length; i++)
            {
                var cell = InventoryData.Cells[i];
                if(!cell.IsAvailable)
                    continue;
                
                if (cell.Item == null)
                {
                    LogService.Log($"Added to slot: {i + 1}, amount: {amount}");
                    cell.Item = new InventoryItemData()
                    {
                        Id = itemId,
                        Amount = amount
                    };

                    return;
                }
            }

            LogService.Warning($"Not enough space in inventory. Remaining: {amount}");
        }
    }
}