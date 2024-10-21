// Created by LunarEclipse on 2024-7-13 6:36.

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace USEN.Games.Roulette
{
    public class RouletteCategory
    {
        [JsonProperty] public string title;
        [JsonProperty] public List<RouletteData> roulettes;
    }
}