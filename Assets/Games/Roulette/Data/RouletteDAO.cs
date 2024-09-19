// Created by LunarEclipse on 2024-7-13 19:34.

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;

namespace USEN.Games.Roulette
{
    public class RouletteDAO
    {
        public const string FILE_NAME = "Roulette.data";
        public const string DEFAULT_DATA_PATH = "DefaultRouletteDataset";
        public static Version Version = new(1, 0, 1);
 
        // Singleton
        public static Task<RouletteDAO> Instance;
        
        private TaskCompletionSource<RouletteDataset> tcs = new();
        
        private RouletteDataset _data;
        
        public RouletteDataset Data => _data;
        

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            Instance = Create();
        }
        
        public static Task<RouletteDAO> Create()
        {
            var dao = new RouletteDAO();
            return dao.tcs.Task.ContinueWith(task => dao);
        }
        
        private RouletteDAO()
        {
            // Load default data from resources.
            var defaultData = Resources.Load<RouletteDataset>(DEFAULT_DATA_PATH);
            
            // Load data from file.
            LoadFromFile().ContinueWith(task =>
            {
                Debug.Log($"[RouletteDAO] Load roulette data from file status: {task.Status}");
                // if (task.Status == TaskStatus.RanToCompletion && task.Result != null)
                if (task.Result != null)
                {
                    var data = task.Result;
                    
                    // Overwrite the default data if version is different.
                    if (data.version == null || Version > data.version)
                    {
                        _data = defaultData;
                        _data.version = Version;
                        SaveToFile();
                    }
                    else _data = data;
                }
                else
                {
                    // Print error message of the task.
                    if (task.Exception != null)
                        Debug.LogError($"[RouletteDAO] Load roulette data from file error: {task.Exception.Message}");
                    
                    _data = defaultData != null ? defaultData : ScriptableObject.CreateInstance<RouletteDataset>();
                    SaveToFile();
                }
                Debug.Log($"[RouletteDAO] Data loaded: {_data.categories.Count} categories.");
                tcs.SetResult(_data);
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
        
        public Task SaveToFile()
        {
            Debug.Log($"[RouletteDAO] Saving roulette data to file.");
#if DEBUG
            string json = null;
            try
            {
                json = JsonConvert.SerializeObject(_data, new VersionConverter());
            }
            catch (JsonException e)
            {
                Debug.LogError($"[RouletteDAO] Serialize roulette data error: {e.Message}");
                return null;
            }
#else
            string json = JsonConvert.SerializeObject(_data, new VersionConverter());
#endif
            var path = Path.Combine(Application.persistentDataPath, FILE_NAME);
            return File.WriteAllTextAsync(path, json).ContinueWith(task =>
            {
                Debug.Log($"[RouletteDAO] Roulette data saved to file: {path}");
            }); 
        }

        public async Task<RouletteDataset> LoadFromFile()
        {
            var path = Path.Combine(Application.persistentDataPath, FILE_NAME);
            Debug.Log($"[RouletteDAO] Loading roulette data from file: {path}");
            if (File.Exists(path))
            {
                string json = await File.ReadAllTextAsync(path);
                Debug.Log($"[RouletteDAO] Json loaded: {json}");
                try
                {
                    var obj = JsonConvert.DeserializeObject<RouletteDataset>(json, new VersionConverter());
                    Debug.Log($"[RouletteDAO] Data loaded: {obj.categories.Count} categories.");
                    return obj;
                }
                catch (JsonException e)
                {
                    Debug.LogError($"[RouletteDAO] Deserialize roulette data error: {e.Message}");
                }
            }
            return null;
        }
        
        public RouletteData GetRandomRoulette()
        {
            if (_data.categories.Count == 0) return null;

            var category = _data.categories.First(); //[UnityEngine.Random.Range(0, _data.categories.Count - 1)];
            return category.roulettes[UnityEngine.Random.Range(0, category.roulettes.Count - 1)];
        }
    }
}