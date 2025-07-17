namespace _Project.Scripts.Services.SaveLoad
{
    public interface ISaveLoadService
    {
        public void Save(string key, object data);
        public T Load<T>(string key);
        public void Clear(string key);
    }
}