using System.Linq;
using _Project.Scripts.Configs.Game;
using _Project.Scripts.Configs.Item;
using _Project.Scripts.Data.Inventory;
using _Project.Scripts.Services.Log;
using _Project.Scripts.UI.Popup;

namespace _Project.Scripts.Inventory
{
    public class InventoryPopupHandler
    {
        private readonly ILogService _logService;
        private readonly InventoryData _inventoryData;
        private readonly GameConfig _gameConfig;

        public InventoryPopupHandler(ILogService logService, InventoryData inventoryData, GameConfig gameConfig)
        {
            _logService = logService;
            _inventoryData = inventoryData;
            _gameConfig = gameConfig;
        }

        public void SetPopupInfo(int index, BasePopupUIView popup)
        {
            var itemId = _inventoryData.Cells[index].Item.Id;
            
            var config = _gameConfig.ItemConfigs.FirstOrDefault(c => c.Id == itemId);
            if (config == null)
            {
                _logService.Error($"Config for item with id {itemId} not found");
                return;
            }

            var weight = config.Weight;
            var max = config.Max;
            var icon = config.Icon;
            var additionalInfo = GetAdditionalInfo(config);
            
            popup.SetInfo(itemId, additionalInfo, weight, max, icon);
        }

        private string GetAdditionalInfo(ItemConfig itemConfig)
        {
            if (itemConfig is WeaponConfig weaponConfig)
                return "Урон: " + weaponConfig.Damage.ToString();
            if (itemConfig is ArmorConfig armorConfig)
                return "Защита: " + armorConfig.Value.ToString();

            return string.Empty;
        }
    }
}