using Luna;
using UnityEngine;

#if UNITY_ANDROID

namespace USEN
{
    public static class AndroidPreferences
    {
        public static string Ssid => Android.GetStatic<string>("com.usen.game.plugin.ConfigManager", "ssid");
        public static string TVIdentifier => Android.GetStatic<string>("com.usen.game.plugin.ConfigManager", "tvIdentifier");
        
        public static void Toast(string message)
        {
            using var test = new AndroidJavaClass("com.usen.game.plugin.Test");
            test.CallStatic("toast", message);
        }
    }
}

#endif