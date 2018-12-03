using System.IO;
using System.Runtime.Serialization.Formatters;
using UnityEngine;
using Newtonsoft.Json;

namespace GameManagement
{
    public static class SaveData
    {
        public static string SavePath = "save.dat";

        private static string _dataPath;

        private static JsonSerializerSettings settings = new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
            TypeNameHandling = TypeNameHandling.All
        };

        private static void CheckDataPath()
        {
            if (string.IsNullOrEmpty(_dataPath))
            {
#if UNITY_EDITOR
                _dataPath = Application.dataPath + "/../saves/";
#else
                _dataPath = Application.persistentDataPath + "/saves/";
#endif
            }
        }

        public static void Save(object data, string name)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
#else
            CheckDataPath();

            var path = _dataPath + name + ".dat";

            Directory.CreateDirectory(Path.GetDirectoryName(path));

            File.WriteAllText(path, JsonConvert.SerializeObject(data, settings));
#endif
        }

        public static T Load<T>(string name)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return default(T);
#else
            CheckDataPath();

            var path = _dataPath + name + ".dat";

            if (!File.Exists(path)) return default(T);

            var data = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<T>(data, settings);
#endif
        }
    }
}