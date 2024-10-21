// Created by LunarEclipse on 2024-6-3 9:20.

using System;
using System.Collections.Generic;
using Luna.Extensions;
using Luna.Extensions.Unity;
using Newtonsoft.Json;
using SQLite;
using UnityEngine;

namespace USEN.Games.Roulette
{
    [Table("roulettes")]
    public class RouletteData
    {
        [PrimaryKey] [AutoIncrement] [JsonIgnore] 
        public int ID { get; set; }

        public string Title { get; set; }
        public long Timestamp { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        public string Category { get; set; }

        public string SectorJson
        {
            get => JsonConvert.SerializeObject(sectors);
            set => sectors = JsonConvert.DeserializeObject<List<RouletteSector>>(value);
        }
        
        public List<RouletteSector> sectors = new();
        
        public RouletteData()
        {
            // ID = Guid.NewGuid().ToString();
        }
        
        // Copy constructor.
        public RouletteData(RouletteData other)
        {
            ID = other.ID;
            Title = other.Title;
            sectors = new();
            for (int i = 0; i < other.sectors.Count; i++)
                sectors.Add(new RouletteSector(other.sectors[i]));
        }

        public void OnValidate()
        {
            for (int i = 0; i < sectors.Count; i++)
            {
                var sector = sectors[i];
                sector.id = sectors.IndexOf(sector);
                sector.color = GetSectorColor(sector.id, sectors.Count);
            }
        }
        
        public static Color GetSectorColor(int index, int count)
        {
            var color = Color.HSVToRGB(Mathf.Pow((1.0f / count * index - 0.02f).Mod(1), 1.35f), 1f, 1f);
            color = color
                .WithSaturation(0.85f * (1f - color.g * 0.2f))
                .WithBrightness(Mathf.Clamp(1.4f * (1f - color.b * 0.5f) * (1f - color.g * 0.3f), 0, 1))
                .WithAlpha(0.75f * (1f - color.b * 0.1f));
            return color;
        }
    }
}