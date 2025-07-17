using UnityEngine;

namespace _Project.Scripts.Configs.Wallet
{
    [CreateAssetMenu(fileName = "WalletConfig", menuName = "Configs/Wallet", order = -80)]
    public class WalletConfig : ScriptableObject
    {
        [field: SerializeField] public int StartCoins { get; private set; }
        [field: SerializeField] public int CoinsPerClick { get; private set; }
    }
}