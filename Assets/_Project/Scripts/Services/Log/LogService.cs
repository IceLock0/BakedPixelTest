using UnityEngine;

namespace _Project.Scripts.Services.Log
{
    public class LogService : ILogService
    {
        public void Log(string message) =>
            Debug.Log(message);

        public void Warning(string message) =>
            Debug.LogWarning(message);

        public void Error(string message) =>
            Debug.LogError(message);
    }
}