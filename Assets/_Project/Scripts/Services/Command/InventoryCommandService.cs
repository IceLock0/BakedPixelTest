using System;
using System.Collections.Generic;
using _Project.Scripts.Inventory.Commands;
using _Project.Scripts.Services.Log;

namespace _Project.Scripts.Services.Command
{
    public class InventoryCommandService : IInventoryCommandService
    {
        private readonly ILogService _logService;
        private readonly Dictionary<Type, IInventoryCommand> _commands = new();

        public event Action Performed;
        
        public InventoryCommandService(ILogService logService)
        {
            _logService = logService;
        }
        
        public void Execute<T>(string itemId, int amount)
        {
            var command = _commands[typeof(T)];
            
            if(command == null)
            {
                _logService.Error("Command not found");
                return;
            }
            
            command.Execute(itemId, amount);
            Performed?.Invoke();
        }

        public void AddCommand(IInventoryCommand command)
        {
            var type = command.GetType(); 
            
            if(!_commands.TryAdd(type, command))
            {
                _logService.Error($"Command {type.Name} has already been added");
                return;
            }

            _logService.Log($"ADD Inventory Command. Type = {type}, command = {command}");
        }
    }
}