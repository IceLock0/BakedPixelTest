using System;
using _Project.Scripts.Configs.Game;
using _Project.Scripts.Data;
using _Project.Scripts.Services.Log;
using _Project.Scripts.Services.SaveLoad;
using _Project.Scripts.UI.Buttons.View;

namespace _Project.Scripts.Services.Wallet
{
    public class WalletService : IWalletService, IDisposable
    {
        private readonly ILogService _logService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly WalletButtonUIView _walletButtonUIView;
        private readonly GameConfig _gameConfig;

        private GameData _gameData;

        private int _coins;

        public WalletService(ILogService logService, ISaveLoadService saveLoadService,
            WalletButtonUIView walletButtonUIView, GameConfig gameConfig)
        {
            _logService = logService;
            _saveLoadService = saveLoadService;
            _walletButtonUIView = walletButtonUIView;
            _gameConfig = gameConfig;
        }

        public void Initialize(GameData gameData)
        {
            _gameData = gameData;

            AddCoins(_gameData.Coins);
            _walletButtonUIView.Clicked += OnAddCoinsClicked;
        }

        public void Dispose()
        {
            _walletButtonUIView.Clicked -= OnAddCoinsClicked;
        }

        public void AddCoins(int amount)
        {
            _coins += amount;
            _walletButtonUIView.SetCoins(_coins);
            
            _gameData.Coins = _coins;
            _saveLoadService.Save(_gameConfig.Id, _gameData);
        }

        public bool TrySpendCoins(int amount)
        {
            if (_coins < amount)
            {
                _logService.Warning("Not enough coins");
                return false;
            }

            _coins -= amount;
            _walletButtonUIView.SetCoins(_coins);

            _gameData.Coins = _coins;
            _saveLoadService.Save(_gameConfig.Id, _gameData);
            
            return true;
        }

        private void OnAddCoinsClicked() =>
            AddCoins(_gameConfig.WalletConfig.CoinsPerClick);
    }
}