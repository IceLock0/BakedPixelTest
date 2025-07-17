using _Project.Scripts.Configs.Inventory;
using _Project.Scripts.Configs.Item;
using _Project.Scripts.Configs.Wallet;
using UnityEngine;

namespace _Project.Scripts.Configs.Game
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Configs/Game", order = -100)]
    public class GameConfig : ScriptableObject
    {
        [field: SerializeField] public string Id { get; private set; }
        [field: SerializeField] public ItemConfig[] ItemConfigs { get; private set; }
        [field: SerializeField] public InventoryConfig InventoryConfig { get; private set; }
        [field: SerializeField] public WalletConfig WalletConfig { get; private set; }
        [field: SerializeField] public bool ClearSave { get; private set; }
    }
}