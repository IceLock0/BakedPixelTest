using _Project.Scripts.Data;

namespace _Project.Scripts.Services.Wallet
{
    public interface IWalletService
    {
        public void Initialize(GameData gameData);
        public void AddCoins(int amount);
        public bool TrySpendCoins(int amount);
    }
}