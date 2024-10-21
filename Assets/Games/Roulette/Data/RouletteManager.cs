// Created by LunarEclipse on 2024-7-13 19:34.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SQLite;
using UnityEngine;

namespace USEN.Games.Roulette
{
    public class RouletteManager
    {
        public static RouletteManager Instance { get; } = new();
        
        public bool IsDirty { get; private set; }
        
        public readonly SQLiteConnection db;
        
        private RouletteManager(string databaseName = null)
        {
            var databasePath = Path.Combine(Application.persistentDataPath, "DB", databaseName ?? "roulette.db");
            var databaseDirectory = Path.GetDirectoryName(databasePath);

            // Create the directory if it doesn't exist.
            if (!Directory.Exists(databaseDirectory))
                Directory.CreateDirectory(databaseDirectory!);

            db = new SQLiteConnection(databasePath);
            db.CreateTable<RouletteData>();
            
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
            return Usen.API.GetRoulettes().ContinueWith(task =>
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

        public RouletteData GetRandomRoulette()
        {
            var data = db.Table<RouletteData>().ToList();
            
            if (data.Count == 0) return null;

            var result =
                from roulette in data
                where roulette.Category == "バツゲーム"
                select roulette;
            var batuGames = result.ToList();
            
            return batuGames[UnityEngine.Random.Range(0, batuGames.Count - 1)];
        }
        
        public void AddRoulette(RouletteData roulette)
        {
            db.Insert(roulette);
            IsDirty = true;
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
        }
        
        public void DeleteRoulette(RouletteData roulette)
        {
            db.Delete(roulette);
            IsDirty = true;
        }
        
        public void DeleteAll()
        {
            db.DeleteAll<RouletteData>();
            IsDirty = true;
        }
    }
}