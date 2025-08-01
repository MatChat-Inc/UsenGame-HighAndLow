#if UNITY_EDITOR

using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class PreBuildProcessor : IPreprocessBuildWithReport
{
    public int callbackOrder => 0;
    
    [InitializeOnLoadMethod]
    public static void OnProjectLoad()
    {
        if (Application.isBatchMode)
        {
            bool isDevBuild = System.Environment.GetCommandLineArgs().ToList().Contains("-development");
            if (isDevBuild)
                EditorUserBuildSettings.development = true;
        }
    }
    
    public static void ProcessDevelopmentBundleVersion()
    {
        if (!PlayerSettings.bundleVersion.EndsWith(".stg"))
        {
            // PlayerSettings.Android.bundleVersionCode++;
            PlayerSettings.bundleVersion += ".stg";
        }
    }
    
    public void OnPreprocessBuild(BuildReport report)
    {
        if (EditorUserBuildSettings.development)
        {
            ProcessDevelopmentBundleVersion();
        }
    }
}

#endif