// Created by LunarEclipse on 2024-09-26 19:09.

using System.Collections.Generic;
using System.Threading.Tasks;
using USEN.Games.Roulette;
using Usen.Models;

namespace Usen
{
    public partial class API
    {
        public static Task<RouletteCategories> GetRouletteCategories()
        {
            return Request.Get<RouletteCategories>("roulette/categories");
        }
        
        public static Task<RouletteCategory> GetRouletteCategory(string category)
        {
            return Request.Get<RouletteCategory>($"/roulette/categories/{category}");
        }
        
        public static Task<AddRouletteResponse> AddRoulette(RouletteData roulette)
        {
            return Request.Post<AddRouletteResponse>("/roulette/roulettes", roulette);
        }
        
        public static Task<Response> UpdateRoulette(RouletteData roulette)
        {
            return Request.Put($"/roulette/roulettes/{roulette.ID}", roulette);
        }
        
        public static Task<Response> DeleteRoulette(string id)
        {
            return Request.Delete($"/roulette/roulettes/{id}");
        }
    }
}