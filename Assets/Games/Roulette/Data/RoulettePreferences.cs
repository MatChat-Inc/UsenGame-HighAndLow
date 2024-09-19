using UnityEngine;

namespace USEN.Games.Roulette
{
    public static class RoulettePreferences
    {
        public static RouletteDisplayMode DisplayMode
        {
            get => (RouletteDisplayMode) PlayerPrefs.GetInt("Roulette.DisplayMode", 0);
            set => PlayerPrefs.SetInt("Roulette.DisplayMode", (int) value);
        }
    }
    
    public enum RouletteDisplayMode
    {
        Normal,
        Random,
    }
}
