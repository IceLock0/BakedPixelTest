namespace _Project.Scripts.Inventory.Commands
{
    public interface IInventoryCommand
    {
        public void Execute(string itemId, int amount);
    }
}