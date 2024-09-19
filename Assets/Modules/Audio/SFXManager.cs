// Created by LunarEclipse on 2024-7-14 7:54.

using System.Collections.Generic;
using UnityEngine;

namespace Luna.UI.Audio
{
    public static class SFXManager
    {
        private static Dictionary<string, AudioClip> soundEffects = new Dictionary<string, AudioClip>();
        private static AudioSource audioSource;

        [RuntimeInitializeOnLoadMethod]
        static void Initialize()
        {
            GameObject soundManagerObject = new GameObject("SFX Manager");
            audioSource = soundManagerObject.AddComponent<AudioSource>();
            Object.DontDestroyOnLoad(soundManagerObject);
        }

        public static void AddSound(string clipName, AudioClip clip)
        {
            if (!soundEffects.ContainsKey(clipName))
            {
                soundEffects[clipName] = clip;
            }
            else
            {
                Debug.LogWarning($"SFXManager: Sound '{clipName}' already exists!");
            }
        }
        
        public static void Play(AudioClip clip)
        {
            audioSource.PlayOneShot(clip);
        }

        public static void Play(string clipName)
        {
            if (soundEffects.ContainsKey(clipName))
            {
                Play(soundEffects[clipName]);
            }
            else
            {
                Debug.LogWarning($"SFXManager: Sound '{clipName}' not found!");
            }
        }
    }   
}