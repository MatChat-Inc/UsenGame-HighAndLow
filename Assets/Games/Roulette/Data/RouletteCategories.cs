// Created by LunarEclipse on 2024-7-13 6:36.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace USEN.Games.Roulette
{
    public class RouletteCategories
    {
        [JsonProperty] public Version version;
        [JsonProperty] public List<RouletteCategory> categories = new();
    }
}