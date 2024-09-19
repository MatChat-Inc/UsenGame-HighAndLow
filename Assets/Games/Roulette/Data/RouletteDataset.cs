// Created by LunarEclipse on 2024-7-13 6:36.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace USEN.Games.Roulette
{
    [JsonObject(MemberSerialization.OptIn)]
    [CreateAssetMenu(fileName = "RouletteDatabase", menuName = "Scriptable Objects/Roulette/Roulette Database")]
    public class RouletteDataset : ScriptableObject
    {
        [JsonProperty] public Version version;
        [JsonProperty] public List<RouletteCategory> categories = new();
    }
}