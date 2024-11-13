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
        
        public static int CommendationVideoOption
        {
            get => PlayerPrefs.GetInt("Roulette.CommendationVideoOption", 0);
            set => PlayerPrefs.SetInt("Roulette.CommendationVideoOption", value);
        }
        
        public static float BgmVolume
        {
            get => PlayerPrefs.GetFloat("Roulette.BgmVolume", 1);
            set => PlayerPrefs.SetFloat("Roulette.BgmVolume", value);
        }
        
        public static float SfxVolume
        {
            get => PlayerPrefs.GetFloat("Roulette.SfxVolume", 1);
            set => PlayerPrefs.SetFloat("Roulette.SfxVolume", value);
        }
    }
    
    public enum RouletteDisplayMode
    {
        Normal,
        Random,
    }
}
