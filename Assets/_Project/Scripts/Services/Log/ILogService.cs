namespace _Project.Scripts.Services.Log
{
    public interface ILogService
    {
        public void Log(string message);
        public void Warning(string message);
        public void Error(string message);
    }
}