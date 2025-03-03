// Created by LunarEclipse on 2024-09-26 19:09.

using System.Collections.Generic;
using System.Threading.Tasks;
using USEN.Games.Roulette;
using USEN.Roulette;

namespace USEN
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
            PreprocessRouletteData(roulette);
            return Request.Post<AddRouletteResponse>($"/roulette/roulettes/{roulette.Category}", roulette);
        }
        
        public static Task<Response> UpdateRoulette(RouletteData roulette)
        {
            PreprocessRouletteData(roulette);
            return Request.Put($"/roulette/roulettes/{roulette.ID}", roulette);
        }
        
        public static Task<Response> DeleteRoulette(string id)
        {
            return Request.Delete($"/roulette/roulettes/{id}");
        }
        
        public static Task<RandomSettingResponse> GetRandomSetting()
        {
            return Request.Get<RandomSettingResponse>("/setting/random");
        }
        
        public static Task<Response> UpdateRandomSetting(bool isRandom)
        {
            return Request.Post("/setting/random", new { random = isRandom ? 1 : 0 });
        }
        
        private static void PreprocessRouletteData(RouletteData roulette)
        {
            foreach (var sector in roulette.sectors)
                if (string.IsNullOrEmpty(sector.content))
                    sector.content = "　";
        }
    }
}