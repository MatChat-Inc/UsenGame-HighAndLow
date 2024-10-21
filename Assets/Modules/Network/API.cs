// Created by LunarEclipse on 2024-09-26 19:09.

using System.Collections.Generic;
using System.Threading.Tasks;
using USEN.Games.Roulette;
using Usen.Network;

namespace Usen
{
    public partial class API
    {
        // Get roulette data with http request.
        public static Task<RouletteCategories> GetRoulettes()
        {
            return Request.Get<RouletteCategories>("get/roulettes");
        }
        
        public static Task<RouletteCategory> GetRouletteCategory(string category)
        {
            return Request.Get<RouletteCategory>($"get/roulette/categories/{category}");
        }
        
        
    }
}