// Created by LunarEclipse on 2024-7-14 8:18.

#if UNITY_EDITOR

using System.Collections.Generic;
using System.IO;
using Luna.UI.Audio.Luna.UI;
using UnityEngine;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;

namespace Luna.UI.Audio
{
    public class AudioPostprocessor : AssetPostprocessor
    {
        const string DB_FILE_PATH = "Audios.g";
        const string CS_FILE_PATH = "Assets/Scripts/R.cs";
        
        const float MAX_SFX_LENGTH = 20f; 
        
        // Find all audio files.
        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths, bool didDomainReload)
        {
            RemoveAllMissingAudios();
            foreach (var asset in importedAssets)
            {
                if (asset.EndsWith(".wav") || asset.EndsWith(".mp3") || asset.EndsWith(".ogg"))
                {
                    Debug.Log("AudioPostprocessor: Found audio clip: " + asset);
                    var audioClip = AssetDatabase.LoadAssetAtPath<AudioClip>(asset);
                    if (audioClip != null)
                    {
                        ProcessAudioClip(audioClip);
                    }
                }
            }
        }
        
        // Find all audio clips when the editor starts or recompiles.
        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            RemoveAllMissingAudios();
            
            // Get the default addressable settings
            AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
            
            var audioClips = new Dictionary<string, AudioClip>();
            
            string[] guids = AssetDatabase.FindAssets("t:AudioClip");
            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                var audioClip = AssetDatabase.LoadAssetAtPath<AudioClip>(path);
                if (audioClip != null)
                {
                    // Debug.Log("AudioPostprocessor: Found audio clip: " + audioClip.name);
                    ProcessAudioClip(audioClip);
                    // var filename = ProcessName(Path.GetFileName(audioClip.name));
                    // if (audioClips.ContainsKey(filename))
                    //     filename = filename + "_" + guid;
                    // audioClips.Add(filename, audioClip);
                    
                    // AddressableAssetEntry entry = settings.CreateOrMoveEntry(AssetDatabase.AssetPathToGUID(path), audioGroup);
                    // entry.address = Path.GetFileNameWithoutExtension(path);
                }
            }
            
            // DeleteGeneratedCode();
            // GenerateCode(audioClips);
        }
        
        private static void ProcessAudioClip(AudioClip audioClip)
        {
            if (audioClip != null)
            {
                // Create the scriptable object if it doesn't exist.
                Audios audios = Resources.Load<Audios>(DB_FILE_PATH);
                if (audios == null)
                {
                    // Create directory if it doesn't exist.
                    if (!AssetDatabase.IsValidFolder("Assets/Resources"))
                        AssetDatabase.CreateFolder("Assets", "Resources");
                
                    audios = ScriptableObject.CreateInstance<Audios>();
                    AssetDatabase.CreateAsset(audios, $"Assets/Resources/{DB_FILE_PATH}.asset");
                }

                if (audioClip.length < MAX_SFX_LENGTH)
                {
                    audios.Add(audioClip);
                    EditorUtility.SetDirty(audios);
                }
            }
        }

        private static void RemoveAllMissingAudios()
        {
            Audios audios = Resources.Load<Audios>(DB_FILE_PATH);
            if (audios != null)
            {
                bool removed = false;
                for (int i = audios.Clips.Count - 1; i >= 0; i--)
                {
                    if (audios.Clips[i] == null)
                    {
                        Debug.Log($"Removed missing prefab from {DB_FILE_PATH}");
                        audios.Clips.RemoveAt(i);
                        removed = true;
                    }
                }

                if (removed)
                {
                    EditorUtility.SetDirty(audios);
                }
            }
        }

        
        private static void LoadGeneratedCode()
        {
            // Load R.cs file.
            if (File.Exists(CS_FILE_PATH))
            {
                Debug.Log("AudioPostprocessor: Loading R.cs file...");
                string code = File.ReadAllText(CS_FILE_PATH);
                Debug.Log(code);
            }
        }
        
        private static void DeleteGeneratedCode()
        {
            // Delete R.cs file.
            if (File.Exists(CS_FILE_PATH))
            {
                Debug.Log("AudioPostprocessor: Deleting old R.cs file...");
                File.Delete(CS_FILE_PATH);
            }
        }
        
        private static void GenerateCode(Dictionary<string, AudioClip> audioClips)
        {
            // Generate R.cs file which contains all the audio clips as constants.
            string code = $@" 
// This file is auto-generated by AudioPostprocessor.cs.
// Do not modify this file manually.

namespace Luna.UI.Audio
{{
    public static class R
    {{
        class Audios
        {{
{GenerateConstants(audioClips)}
        }}
    }}
}}
";
            Debug.Log("AudioPostprocessor: Generating R.cs file...");
            System.IO.File.WriteAllText(CS_FILE_PATH, code);
        }

        private static string GenerateConstants(Dictionary<string, AudioClip> audioClips)
        {
            string code = "";
            foreach (var pair in audioClips)
            {
                Debug.Log("AudioPostprocessor: Generating constant for " + pair.Value.name);
                code += $@"            public const string {pair.Key} = ""{pair.Value.name}"";" + "\n";
            }
            return code;
        }
        
        private static string ProcessName(string oldName)
        {
            var name = oldName;
            
            // If starts with a number, add an underscore.
            if (char.IsDigit(name[0]))
                name = "_" + name;
            
            return name
                .Replace(" ", "_")
                .Replace("-", "_")
                .Replace("(", "_")
                .Replace(")", "_")
                .Replace("'", "");
        }
    }
}

#endif
