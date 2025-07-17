using System;
using _Project.Scripts.Inventory.Commands;

namespace _Project.Scripts.Services.Command
{
    public interface IInventoryCommandService
    {
        public void Execute<T>(string itemId, int amount);
        public void AddCommand(IInventoryCommand command);

        public event Action Performed;
    }
}