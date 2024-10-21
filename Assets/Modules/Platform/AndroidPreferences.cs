using Luna;
using UnityEngine;

#if UNITY_ANDROID

namespace USEN
{
    public static class AndroidPreferences
    {
        public static string TVIdentifier => Android.GetStatic<string>("com.usen.game.plugin.ConfigManager", "tvIdentifier");
        // {
        //     get
        //     {
        //         using var configManager = new AndroidJavaClass("com.usen.game.plugin.ConfigManager");
        //         return configManager.GetStatic<string>("tvIdentifier");
        //     }
        // }
        
        public static void Toast(string message)
        {
            using var test = new AndroidJavaClass("com.usen.game.plugin.Test");
            test.CallStatic("toast", message);
        }
    }
}

#endif
