using System.IO;
using UnityEngine;
using Newtonsoft.Json;

namespace GameManagement
{
    public static class SaveData
    {
        public static string SavePath = "save.dat";

        private static string _dataPath;

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
            CheckDataPath();

            var path = _dataPath + name + ".dat";

            Directory.CreateDirectory(Path.GetDirectoryName(path));    

            File.WriteAllText(path, JsonConvert.SerializeObject(data));
        }

        public static T Load<T>(string name)
        {
            CheckDataPath();

            var path = _dataPath + name + ".dat";

            if (!File.Exists(path)) return default(T);

            var data = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<T>(data);
        }
    }
}