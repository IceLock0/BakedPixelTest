using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Configs.Game;
using _Project.Scripts.Configs.Item;
using _Project.Scripts.Extensions;
using _Project.Scripts.Inventory.Commands;
using _Project.Scripts.Services.Command;
using _Project.Scripts.Services.Log;
using _Project.Scripts.UI.Buttons.View;
using Zenject;

namespace _Project.Scripts.Inventory
{
    public class InventoryButtonsHandler : IInitializable, IDisposable
    {
        private readonly IInventoryCommandService _inventoryCommandService;
        private readonly ILogService _logService;
        private readonly GameConfig _gameConfig;
        private readonly ShootButtonUIView _shootButton;
        private readonly AddAmmoButtonUIView _addAmmoButton;
        private readonly AddItemButtonUIView _addItemButton;
        private readonly RemoveItemButtonUIView _removeItemButton;

        public event Action ShootProcessed; 
        public event Action RemoveItemProcessed; 
        
        public InventoryButtonsHandler(IInventoryCommandService inventoryCommandService, ILogService logService,
            GameConfig gameConfig,
            ShootButtonUIView shootButton, AddAmmoButtonUIView addAmmoButton,
            AddItemButtonUIView addItemButton, RemoveItemButtonUIView removeItemButton)
        {
            _inventoryCommandService = inventoryCommandService;
            _logService = logService;
            _gameConfig = gameConfig;
            _shootButton = shootButton;
            _addAmmoButton = addAmmoButton;
            _addItemButton = addItemButton;
            _removeItemButton = removeItemButton;
        }

        public void Initialize()
        {
            _shootButton.Clicked += OnShootClicked;
            _addAmmoButton.Clicked += OnAddAmmoClicked;
            _addItemButton.Clicked += OnAddItemClicked;
            _removeItemButton.Clicked += OnRemoveItemClicked;
        }

        public void Dispose()
        {
            _shootButton.Clicked -= OnShootClicked;
            _addAmmoButton.Clicked -= OnAddAmmoClicked;
            _addItemButton.Clicked -= OnAddItemClicked;
            _removeItemButton.Clicked -= OnRemoveItemClicked;
        }

        public List<T> GetConfigs<T>() where T : ItemConfig
        {
            var configs = new List<T>();
            foreach (var itemConfig in _gameConfig.ItemConfigs)
            {
                if (itemConfig is T config)
                    configs.Add(config);
            }

            if (configs.Count == 0)
                _logService.Error($"Couldnt find configs for type {typeof(T)}");

            return configs;
        }
        
        private void OnShootClicked() =>
            ProcessShoot();

        private void OnAddAmmoClicked() =>
            ProcessAddAmmo();

        private void OnAddItemClicked() =>
            ProcessAddItem();

        private void OnRemoveItemClicked() =>
            ProcessRemoveItem();

        private void ProcessShoot() =>
            ShootProcessed?.Invoke();

        private void ProcessAddAmmo()
        {
            var ammoConfigs = GetConfigs<AmmoConfig>();
            foreach (var ammoConfig in ammoConfigs)
                _inventoryCommandService.Execute<AddItemInventoryCommand>(ammoConfig.Id, 30);
        }

        private void ProcessAddItem()
        {
            var itemConfigs = GetConfigs<WeaponConfig>()
                .Cast<ItemConfig>()
                .Concat(GetConfigs<ArmorConfig>())
                .ToList();

            var rnd = itemConfigs.GetRandom();
            _inventoryCommandService.Execute<AddItemInventoryCommand>(rnd.Id, 1);
        }

        private void ProcessRemoveItem() =>
            RemoveItemProcessed?.Invoke();
    }
}