using System;
using _Project.Scripts.Configs.Game;
using _Project.Scripts.Data;
using _Project.Scripts.Factories.Inventory;
using _Project.Scripts.Inventory;
using _Project.Scripts.Services.Log;
using _Project.Scripts.Services.SaveLoad;
using _Project.Scripts.Services.Wallet;
using Zenject;

namespace _Project.Scripts.EntryPoint
{
    public class GameplayEntryPoint : IInitializable, IDisposable
    {
        private readonly ILogService _logService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IInventoryFactory _inventoryFactory;
        private readonly IWalletService _walletService;
        private readonly GameConfig _gameConfig;

        private GameData _gameData;
        private InventoryController _inventoryController;

        public GameplayEntryPoint(ILogService logService, ISaveLoadService saveLoadService,
            IInventoryFactory inventoryFactory, IWalletService walletService, GameConfig gameConfig)
        {
            _logService = logService;
            _saveLoadService = saveLoadService;
            _inventoryFactory = inventoryFactory;
            _walletService = walletService;
            _gameConfig = gameConfig;
        }

        public void Initialize()
        {
            _logService.Log("INIT GameplayEntryPoint");

            LoadGameData();

            CreateInventory();

            _inventoryController.Initialize(_gameData);

            _walletService.Initialize(_gameData);
        }

        public void Dispose()
        {
            _logService.Log("DISPOSE GameplayEntryPoint");

            _inventoryController.Dispose();
        }

        private void LoadGameData()
        {
            if (_gameConfig.ClearSave)
                _saveLoadService.Clear(_gameConfig.Id);

            _gameData = _saveLoadService.Load<GameData>(_gameConfig.Id) ?? new GameData
            {
                Coins = _gameConfig.WalletConfig.StartCoins
            };
        }
        
        private void CreateInventory()
        {
            var inventoryData = _gameData.InventoryData;
            
            _inventoryController = _inventoryFactory.Create(inventoryData);
        }
    }
}