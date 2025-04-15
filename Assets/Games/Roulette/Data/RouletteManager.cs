// Created by LunarEclipse on 2024-7-13 19:34.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SQLite;
using UnityEngine;
using USEN;
using Random = System.Random;

namespace USEN.Games.Roulette
{
    public class RouletteManager
    {
        public static RouletteManager Instance { get; } = new();
        
        public bool IsDirty { get; private set; }
        
        public readonly SQLiteConnection db;

        private int _version = 1;
        
        private List<int> _previousRandomIndexes = new();
        
        private RouletteManager(string databaseName = null)
        {
            var databasePath = Path.Combine(Application.persistentDataPath, "DB", databaseName ?? $"roulette.db");
            var databaseDirectory = Path.GetDirectoryName(databasePath);

            // Create the directory if it doesn't exist.
            if (!Directory.Exists(databaseDirectory))
                Directory.CreateDirectory(databaseDirectory!);

            db = new SQLiteConnection(databasePath);
            db.CreateTable<RouletteData>();
            
            UpdateTable();
            
            if (!db.Table<RouletteData>().Any())
            {
                var json = Resources.Load<TextAsset>("roulette").text;
                
                Debug.Log($"[RouletteManager] Database is empty. Inserting default data: \n{json}");
                
                var categories = JsonConvert.DeserializeObject<RouletteCategories>(json);
                
                Debug.Log($"[RouletteManager] Inserting {categories.categories.Sum(c => c.roulettes.Count)} roulettes.");
                
                db.RunInTransaction(() => {
                    foreach (var category in categories.categories)
                        foreach (var roulette in category.roulettes)
                        {
                            roulette.Category = category.title;
                            db.Insert(roulette);
                        }
                });
            }
        }
        
        public Task<RouletteCategories> Sync()
        {
            return USEN.API.GetRouletteCategories().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogWarning($"[RouletteManager] Sync failed. Use local data as fallback.");
                    return new RouletteCategories()
                    {
                        categories = GetCategories()
                    };
                }

                var categories = task.Result.categories;
                db.RunInTransaction(() =>
                {
                    db.DeleteAll<RouletteData>();
                    foreach (var category in categories)
                        foreach (var roulette in category.roulettes)
                        {
                            roulette.Category = category.title;
                            db.Insert(roulette);
                        }
                });
                
                IsDirty = false;
                
                return task.Result;
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public List<RouletteCategory> GetCategories()
        {
            var data = db.Table<RouletteData>().ToList();
            var categories =
                from roulette in data
                group roulette by roulette.Category into g
                select new RouletteCategory
                {
                    title = g.Key,
                    roulettes = g.ToList()
                };
            
            var list = categories.ToList();
            
            // Add original category if not exists.
            if (list.All(c => c.title != "オリジナル"))
            {
                list.Add(new RouletteCategory
                {
                    title = "オリジナル",
                    roulettes = new List<RouletteData>()
                });
            }
            
            return list;
        }
        
        public RouletteCategory GetCategory(string title)
        {
            var data = db.Table<RouletteData>().ToList();
            var roulettes =
                from roulette in data
                where roulette.Category == title
                select roulette;
            
            return new RouletteCategory
            {
                title = title,
                roulettes = roulettes.ToList()
            };
        }

        public RouletteData GetRandomRoulette(string fromCategory = "バツゲーム", int notRepeatCount = 10)
        {
            var data = db.Table<RouletteData>().ToList();
            if (data.Count == 0) return null;

            var result =
                from roulette in data
                where roulette.Category == fromCategory
                select roulette;
            var batuGames = result.ToList();
            
            if (_previousRandomIndexes.Count >= batuGames.Count)
                _previousRandomIndexes.Clear();
            
            int nextIndex;
            Random random = new();
            do {
                nextIndex = random.Next(batuGames.Count);
                Debug.Log($"[RouletteManager] Random index: {nextIndex}");
            } while (_previousRandomIndexes.Contains(nextIndex));
            
            _previousRandomIndexes.Add(nextIndex);
            if (_previousRandomIndexes.Count >= Mathf.Min(notRepeatCount, batuGames.Count))
                _previousRandomIndexes.RemoveAt(0);
            
            return batuGames[nextIndex];
        }
        
        public async Task AddRoulette(RouletteData roulette)
        {
            try {
                db.Insert(roulette);
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[RouletteManager] Add failed: {e.Message}");
            }

            IsDirty = true;
            var response = await API.AddRoulette(roulette);
            
            // Update ID with the response.
            roulette.ID = response.id;
            db.Update(roulette);
            
            IsDirty = false;
        }
        
        public void InsertFromJsonList(string json)
        {
            var questions = JsonConvert.DeserializeObject<List<RouletteData>>(json);

            // New transaction to add all questions at once.
            db.RunInTransaction(() =>
            {
                foreach (var question in questions)
                    db.Insert(question);
            });
            
            IsDirty = true;
        }
        
        public void UpdateRoulette(RouletteData roulette)
        {
            db.Update(roulette);
            IsDirty = true;
            API.UpdateRoulette(roulette).ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogWarning($"[RouletteManager] Update failed: {task.Exception}");
                    IsDirty = false;
                }
            });
        }
        
        public void DeleteRoulette(RouletteData roulette)
        {
            db.Delete(roulette);
            IsDirty = true;
            API.DeleteRoulette(roulette.ID).ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogWarning($"[RouletteManager] Delete failed: {task.Exception}");
                    IsDirty = false;
                }
            });
        }
        
        public void DeleteAll()
        {
            db.DeleteAll<RouletteData>();
            IsDirty = true;
        }
        
        public bool ExistsRoulette(RouletteData roulette)
        {
            return db.Table<RouletteData>().Any(r => r.ID == roulette.ID);
        }
        
        private void UpdateTable()
        {
            var tableName = db.GetMapping<RouletteData>().TableName;
            
            // Check version and update database if necessary.
            var version = db.ExecuteScalar<int>("PRAGMA user_version");
            if (version < _version)
            {
                Debug.LogWarning($"[RouletteManager] Database version is outdated. Updating from {version} to {_version}.");

                // Copy old data to new table.
                try
                {
                    db.RunInTransaction(() =>
                    {
                        // Rename old table.
                        db.Execute($"ALTER TABLE {tableName} RENAME TO old_{tableName}");

                        // Create new table.
                        db.CreateTable<RouletteData>();

                        var oldData = db.Query<OldRouletteData>($"SELECT * FROM old_{tableName}");
                        foreach (var old in oldData)
                        {
                            var roulette = new RouletteData(old);
                            db.Insert(roulette);
                        }

                        // Drop old table.
                        db.Execute($"DROP TABLE old_{tableName}");
                    });
                }
                catch (Exception e)
                {
                    Debug.LogError($"[RouletteManager] Failed to update database: {e.Message}");
                }
            }
            
            // Update version.
            db.Execute($"PRAGMA user_version = {_version}");
        }
        
    }
}