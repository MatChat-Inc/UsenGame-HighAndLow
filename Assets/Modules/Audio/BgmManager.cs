// Created by LunarEclipse on 2024-7-30 17:59.

using DG.Tweening;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Luna.UI.Audio
{
    public static class BgmManager
    {
        private static AudioSource audioSource;

        [RuntimeInitializeOnLoadMethod]
        static void Initialize()
        {
            GameObject soundManagerObject = new GameObject("BGM Manager");
            audioSource = soundManagerObject.AddComponent<AudioSource>();
            audioSource.loop = true;
            Object.DontDestroyOnLoad(soundManagerObject);
        }
        
        public static void Play(AudioClip clip)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
        
        public static void Play(AssetReferenceT<AudioClip> clipRef)
        {
            var previousClip = audioSource.clip;
            
            clipRef.LoadAssetAsync().Completed += handle =>
            {
                Play(handle.Result);
                Addressables.Release(previousClip);
            };
        }
        
        public static void Stop()
        {
            audioSource.Stop();
        }
        
        public static void Pause()
        {
            audioSource.Pause();
        }
        
        public static void Resume()
        {
            audioSource.UnPause();
        }
        
        public static void Fade(float volume, float duration = 0.5f)
        {
            DOTween.To(() => audioSource.volume, x => audioSource.volume = x, volume, duration);
        }
    }
}