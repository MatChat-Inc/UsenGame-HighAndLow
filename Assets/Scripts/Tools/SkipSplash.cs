/* ---------------------------------------------------------------- */
/*                    Skip Unity Splash Screen                      */
/*                      Create by psygames                          */
/*            https://github.com/psygames/UnitySkipSplash           */
/* ---------------------------------------------------------------- */

// #if !UNITY_EDITOR
// using UnityEngine;
// using UnityEngine.Rendering;

// public class SkipSplash
// {
//     [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
//     private static void BeforeSplashScreen()
//     {
//         System.Threading.Tasks.Task.Run(AsyncSkip);
//     }

//     private static void AsyncSkip()
//     {
//         SplashScreen.Stop(SplashScreen.StopBehavior.StopImmediate);
//     }
// }
// #endif