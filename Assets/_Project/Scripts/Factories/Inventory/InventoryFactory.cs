using _Project.Scripts.Configs.Game;
using _Project.Scripts.Data.Inventory;
using _Project.Scripts.Inventory;
using _Project.Scripts.Inventory.Commands;
using _Project.Scripts.Services.Command;
using _Project.Scripts.Services.Log;
using _Project.Scripts.Services.SaveLoad;
using _Project.Scripts.Services.Wallet;
using _Project.Scripts.UI.Inventory;
using _Project.Scripts.UI.Inventory.Weight;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Factories.Inventory
{
    public class InventoryFactory : IInventoryFactory
    {
        private readonly IInstantiator _instantiator;
        private readonly ILogService _logService;
        private readonly IInventoryCommandService _inventoryCommandService;
        private readonly IWalletService _walletService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly GameConfig _gameConfig;
        private readonly InventoryUIView _inventoryUIViewPrefab;
        private readonly InventoryWeightUIView _inventoryWeightUIView;
        private readonly InventoryCellUIView _inventoryCellUIViewPrefab;
        private readonly InventoryButtonsHandler _inventoryButtonsHandler;
        private readonly Canvas _uiCanvas;

        private InventoryController _inventoryController;

        public InventoryFactory(IInstantiator instantiator, ILogService logService,
            IInventoryCommandService inventoryCommandService, IWalletService walletService,
            ISaveLoadService saveLoadService, GameConfig gameConfig,
            InventoryUIView inventoryUIViewPrefab, InventoryWeightUIView inventoryWeightUIView,
            InventoryCellUIView inventoryCellUIViewPrefab,
            InventoryButtonsHandler inventoryButtonsHandler, Canvas uiCanvas)
        {
            _instantiator = instantiator;
            _logService = logService;
            _inventoryCommandService = inventoryCommandService;
            _walletService = walletService;
            _saveLoadService = saveLoadService;
            _gameConfig = gameConfig;
            _inventoryUIViewPrefab = inventoryUIViewPrefab;
            _inventoryWeightUIView = inventoryWeightUIView;
            _inventoryCellUIViewPrefab = inventoryCellUIViewPrefab;
            _inventoryButtonsHandler = inventoryButtonsHandler;
            _uiCanvas = uiCanvas;
        }

        public InventoryController Create(InventoryData sourceData)
        {
            _logService.Log("Inventory Factory. CREATE InventoryController");

            InventoryData data = null;
            if (sourceData == null)
                data = CreateInventoryData();
            else
            {
                data = sourceData;
                foreach (var cell in data.Cells)
                {
                    if (cell.Item.Id == "")
                        cell.Item = null;
                }
            }

            var view = CreateInventoryView(data);

            CreateCommands(data);
            
            _inventoryController = CreateInventoryController(data, view);

            return _inventoryController;
        }

        private InventoryUIView CreateInventoryView(InventoryData data)
        {
            _logService.Log($"CREATE InventoryUIView, PREFAB = {_inventoryUIViewPrefab.gameObject.name}");
            var cells = new InventoryCellUIView[_gameConfig.InventoryConfig.Size];

            var container = _uiCanvas.transform.GetChild(0);
            var view = _instantiator.InstantiatePrefabForComponent<InventoryUIView>(_inventoryUIViewPrefab, container);

            for (var i = 0; i < cells.Length; i++)
            {
                var cell = CreateInventoryCellView(view.transform.GetChild(0));

                cell.SetAvailable(data.Cells[i].IsAvailable);

                cells[i] = cell;
            }

            view.Construct(cells, _inventoryWeightUIView);

            return view;
        }

        private InventoryCellUIView CreateInventoryCellView(Transform container)
        {
            _logService.Log($"CREATE CellUIView. Prefab = {_inventoryCellUIViewPrefab.gameObject.name}");

            var cell = _instantiator.InstantiatePrefabForComponent<InventoryCellUIView>(_inventoryCellUIViewPrefab,
                container);

            return cell;
        }

        private InventoryData CreateInventoryData()
        {
            _logService.Log("CREATE InventoryData");

            var data = new InventoryData
            {
                Cells = new InventoryCellData[_gameConfig.InventoryConfig.Size]
            };

            for (var i = 0; i < _gameConfig.InventoryConfig.Size; i++)
            {
                var cellAvailable = i < _gameConfig.InventoryConfig.OpenedCells;

                var cell = new InventoryCellData() { Item = null, IsAvailable = cellAvailable };
                data.Cells[i] = cell;
            }

            return data;
        }

        private void CreateCommands(InventoryData inventoryData)
        {
            _inventoryCommandService.AddCommand(new AddItemInventoryCommand(_logService, inventoryData,
                _gameConfig.ItemConfigs));
            _inventoryCommandService.AddCommand(new RemoveItemInventoryCommand(_logService, inventoryData,
                _gameConfig.ItemConfigs));
        }

        private InventoryController CreateInventoryController(InventoryData data, InventoryUIView view)
        {
            var popupHandler = new InventoryPopupHandler(_logService, data, _gameConfig);
            
            _inventoryController = new InventoryController(_logService, _inventoryCommandService, _walletService,
                _saveLoadService, popupHandler, _inventoryButtonsHandler, _gameConfig, data, view);

            return _inventoryController;
        }
    }
}