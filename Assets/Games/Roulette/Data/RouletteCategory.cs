// Created by LunarEclipse on 2024-7-13 6:36.

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace USEN.Games.Roulette
{
    [JsonObject(MemberSerialization.OptIn)]
    [CreateAssetMenu(fileName = "RouletteCategory", menuName = "Scriptable Objects/Roulette/Roulette Category")]
    public class RouletteCategory : ScriptableObject
    {
        [JsonProperty] public string title;
        [JsonProperty] public List<RouletteData> roulettes;
    }
}