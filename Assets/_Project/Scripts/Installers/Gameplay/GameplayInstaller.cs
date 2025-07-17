using _Project.Scripts.Configs.Game;
using _Project.Scripts.EntryPoint;
using _Project.Scripts.Factories.Inventory;
using _Project.Scripts.Inventory;
using _Project.Scripts.Services.Command;
using _Project.Scripts.Services.Wallet;
using _Project.Scripts.UI.Buttons.View;
using _Project.Scripts.UI.Inventory;
using _Project.Scripts.UI.Inventory.Weight;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Installers.Gameplay
{
    public class GameplayInstaller : MonoInstaller
    {
        [Header("Configs")]
        [SerializeField] private GameConfig _gameConfig;

        [Space(3)] 
        [Header("Prefabs")] 
        [SerializeField] private InventoryUIView _inventoryUIViewPrefab;
        [SerializeField] private InventoryCellUIView _inventoryCellUIPrefab;

        [Space(3)] 
        [Header("Utils")] 
        [SerializeField] private Canvas _uiCanvas;

        [SerializeField] private WalletButtonUIView _walletButtonUIView;

        [SerializeField] private ShootButtonUIView _shootButtonUIView;
        [SerializeField] private AddAmmoButtonUIView _addAmmoButtonUIView;
        [SerializeField] private AddItemButtonUIView _addItemButtonUIView;
        [SerializeField] private RemoveItemButtonUIView _removeItemButtonUIView;

        [SerializeField] private InventoryWeightUIView _inventoryWeightUIView;

        public override void InstallBindings()
        {
            BindGameplayEntryPoint();

            BindServices();

            BindConfigs();

            BindUtils();

            BindFactories();
        }

        private void BindGameplayEntryPoint() =>
            Container.BindInterfacesTo<GameplayEntryPoint>().AsSingle();

        private void BindConfigs()
        {
            Container.Bind<GameConfig>().FromInstance(_gameConfig).AsSingle();
        }

        private void BindServices()
        {
            Container.Bind<IInventoryCommandService>().To<InventoryCommandService>().AsSingle();
            Container.BindInterfacesAndSelfTo<WalletService>().AsSingle().WithArguments(_walletButtonUIView);
        }

        private void BindUtils()
        {
            Container.BindInterfacesAndSelfTo<InventoryButtonsHandler>().AsSingle().WithArguments(_shootButtonUIView, _addAmmoButtonUIView,
                _addItemButtonUIView, _removeItemButtonUIView);
        }

        private void BindFactories()
        {
            Container.BindInterfacesAndSelfTo<InventoryFactory>().AsSingle().WithArguments(_inventoryUIViewPrefab,
                _inventoryWeightUIView, _inventoryCellUIPrefab, _uiCanvas);
        }
    }
}