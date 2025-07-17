using System.IO;
using UnityEngine;

namespace _Project.Scripts.Services.SaveLoad
{
    public class JsonSaveLoadService : ISaveLoadService
    {
        public void Save(string key, object data)
        {
            var path = GetPath(key);

            if (!File.Exists(path))
                File.Create(path).Close();

            var json = JsonUtility.ToJson(data);

            File.WriteAllText(path, json);
        }

        public T Load<T>(string key)
        {
            var path = GetPath(key);

            if (!File.Exists(path))
                return default;

            var json = File.ReadAllText(path);

            return JsonUtility.FromJson<T>(json);
        }

        public void Clear(string key)
        {
            var path = GetPath(key);

            if (!File.Exists(path))
                return;
            
            File.Delete(path);
        }

        private string GetPath(string key) => 
            Path.Combine(Application.persistentDataPath, key) + ".json";
    }
}