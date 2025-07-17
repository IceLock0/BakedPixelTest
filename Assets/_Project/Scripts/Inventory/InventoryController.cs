using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Configs.Game;
using _Project.Scripts.Configs.Item;
using _Project.Scripts.Data;
using _Project.Scripts.Data.Inventory;
using _Project.Scripts.Extensions;
using _Project.Scripts.Inventory.Commands;
using _Project.Scripts.Services.Command;
using _Project.Scripts.Services.Log;
using _Project.Scripts.Services.SaveLoad;
using _Project.Scripts.Services.Wallet;
using _Project.Scripts.UI.Inventory;
using _Project.Scripts.UI.Popup;
using UnityEngine.UI;

namespace _Project.Scripts.Inventory
{
    public class InventoryController : IDisposable
    {
        private readonly ILogService _logService;
        private readonly IInventoryCommandService _inventoryCommandService;
        private readonly IWalletService _walletService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly InventoryPopupHandler _inventoryPopupHandler;
        private readonly InventoryButtonsHandler _inventoryButtonsHandler;
        private readonly GameConfig _gameConfig;
        private readonly InventoryData _inventoryData;
        private readonly InventoryUIView _inventoryUIView;

        private GameData _gameData;

        public InventoryController(ILogService logService, IInventoryCommandService inventoryCommandService,
            IWalletService walletService, ISaveLoadService saveLoadService,
            InventoryPopupHandler inventoryPopupHandler, InventoryButtonsHandler inventoryButtonsHandler,
            GameConfig gameConfig, InventoryData inventoryData, InventoryUIView inventoryUIView)
        {
            _logService = logService;
            _inventoryCommandService = inventoryCommandService;
            _walletService = walletService;
            _saveLoadService = saveLoadService;
            _inventoryPopupHandler = inventoryPopupHandler;
            _inventoryButtonsHandler = inventoryButtonsHandler;
            _gameConfig = gameConfig;
            _inventoryData = inventoryData;
            _inventoryUIView = inventoryUIView;
        }

        public void Initialize(GameData gameData)
        {
            _gameData = gameData;
            _gameData.InventoryData = _inventoryData;

            _inventoryCommandService.Performed += OnInventoryCommandPerformed;
            _inventoryButtonsHandler.ShootProcessed += OnShootProcessed;
            _inventoryButtonsHandler.RemoveItemProcessed += OnRemoveItemProcessed;
            _inventoryUIView.UnblockCellClicked += OnUnblockCellInInventoryClicked;
            _inventoryUIView.PopupCreated += OnPopupCreated;

            Refresh();
        }

        public void Dispose()
        {
            _inventoryCommandService.Performed -= OnInventoryCommandPerformed;
            _inventoryButtonsHandler.ShootProcessed -= OnShootProcessed;
            _inventoryButtonsHandler.RemoveItemProcessed -= OnRemoveItemProcessed;
            _inventoryUIView.UnblockCellClicked -= OnUnblockCellInInventoryClicked;
            _inventoryUIView.PopupCreated -= OnPopupCreated;
        }

        private void OnInventoryCommandPerformed() =>
            Refresh();

        private void OnShootProcessed() =>
            ProcessShoot();

        private void OnRemoveItemProcessed() =>
            ProcessRemoveItem();

        private void OnUnblockCellInInventoryClicked(int index) =>
            UnlockCell(index);

        private void OnPopupCreated(int index, BasePopupUIView popup) =>
            SetPopupInfo(index, popup);

        private void Refresh()
        {
            var weight = 0.0f;
            for (var i = 0; i < _inventoryData.Cells.Length; i++)
            {
                var cell = _inventoryData.Cells[i];
                var item = cell.Item;

                if (item == null)
                    _inventoryUIView.SetCellData(i);
                else
                {
                    _inventoryUIView.SetCellData(i, GetItemIcon(item.Id), item.Amount);
                    var config = _gameConfig.ItemConfigs.FirstOrDefault(c => c.Id == item.Id);
                    if (config != null)
                        weight += config.Weight * item.Amount;
                }
            }

            _inventoryUIView.SetInventoryData(weight);

            _saveLoadService.Save(_gameConfig.Id, _gameData);
        }

        private Image GetItemIcon(string id)
        {
            var itemConfig = _gameConfig.ItemConfigs.FirstOrDefault(c => c.Id == id);
            if (itemConfig == null)
            {
                _logService.Error($"Could not find item config for item with id: {id}");
                return null;
            }

            return itemConfig.Icon;
        }

        private void ProcessShoot()
        {
            var weaponConfigs = _inventoryButtonsHandler.GetConfigs<WeaponConfig>();
            var weaponsId = weaponConfigs.ToDictionary(c => c.Id);

            var weaponsInInventory = new List<string>();

            foreach (var cell in _inventoryData.Cells)
            {
                var item = cell.Item;
                if (item != null && weaponsId.ContainsKey(item.Id) && !weaponsInInventory.Contains(item.Id))
                    weaponsInInventory.Add(item.Id);
            }

            if (weaponsInInventory.Count == 0)
            {
                _logService.Error($"No weapons in inventory to shoot");
                return;
            }

            var rndWeapon = weaponsInInventory.GetRandom();
            if (!weaponsId.TryGetValue(rndWeapon, out var weapon))
            {
                _logService.Error($"Couldnt find ammo for weapon with id {rndWeapon}");
                return;
            }

            _logService.Log(
                $"Try shoot. Weapon info: with id = {rndWeapon}, ammo id = {weapon.Ammo.Id}, damage = {weapon.Damage}");
            _inventoryCommandService.Execute<RemoveItemInventoryCommand>(weapon.Ammo.Id, 1);
        }

        private void ProcessRemoveItem()
        {
            var existingCells = _inventoryData.Cells.Where(cell => cell.Item != null).ToList();

            if (existingCells.Count == 0)
            {
                _logService.Warning($"Nothing to remove");
                return;
            }

            var rndCell = existingCells.GetRandom();

            _inventoryCommandService.Execute<RemoveItemInventoryCommand>(rndCell.Item.Id, rndCell.Item.Amount);
        }

        private void UnlockCell(int index)
        {
            if (!_walletService.TrySpendCoins(_gameConfig.InventoryConfig.CellPrice))
            {
                _logService.Warning($"Not enough money for unblock cell with id: {index + 1}");
                return;
            }

            _inventoryData.Cells[index].IsAvailable = true;
            _inventoryUIView.SetCellAvailable(index, true);

            _logService.Log($"Unblocked cell with id {index + 1}");
        }

        private void SetPopupInfo(int index, BasePopupUIView popupUIView) =>
            _inventoryPopupHandler.SetPopupInfo(index, popupUIView);
    }
}