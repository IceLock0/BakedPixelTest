using System;
using _Project.Scripts.Data.Inventory;

namespace _Project.Scripts.Data
{
    [Serializable]
    public class GameData
    {
        public InventoryData InventoryData;
        public int Coins;
    }
}