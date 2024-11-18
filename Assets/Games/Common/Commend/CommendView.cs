// Created by LunarEclipse on 2024-7-19 14:12.

using System;
using System.Collections.Generic;
using Luna;
using Luna.UI;
using Luna.UI.Navigation;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Video;
using USEN.Games.Roulette;

namespace USEN.Games.Common
{
    public class CommendView : Widget
    {
        public VideoPlayer videoPlayer;
        public AudioSource audioSource;
        public BottomPanel bottomPanel;
        
        public List<VideoClip> videoClips;
        public List<AssetReferenceT<AudioClip>> audioClips;

        // public int index;

        private void Start()
        {
            // Play video
            var index = RoulettePreferences.CommendationVideoOption;
             
            videoPlayer.targetCamera = Camera.main;
            
            if (index < videoClips.Count)
            {
                videoPlayer.clip = videoClips[index];
                videoPlayer.Prepare();
            }
            
            videoPlayer.prepareCompleted += OnVideoPrepared;
            videoPlayer.loopPointReached += OnVideoEnd;
            
            // Play audio
            if (index < audioClips.Count)
            {
                audioClips[index].LoadAssetAsync().Completed += handle =>
                {
                    audioSource.clip = handle.Result;
                    audioSource.Play();
                };
            }
        }

        private void OnEnable()
        {
            bottomPanel.onRedButtonClicked += OnRedButtonClicked;
            bottomPanel.onBlueButtonClicked += OnBlueButtonClicked;
            bottomPanel.onGreenButtonClicked += OnGreenButtonClicked;
        }

        private void OnDisable()
        {
            bottomPanel.onRedButtonClicked -= OnRedButtonClicked;
            bottomPanel.onBlueButtonClicked -= OnBlueButtonClicked;
            bottomPanel.onGreenButtonClicked -= OnGreenButtonClicked;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) ||
                Input.GetButtonDown("Cancel")) {
                Navigator.Pop();
            }
        }

        private void OnDestroy()
        {
            // Release assets
            foreach (var audioClip in audioClips)
                audioClip.ReleaseAsset();
        }

        private void OnVideoPrepared(VideoPlayer source)
        {
            videoPlayer.Play();
        }

        private void OnVideoEnd(VideoPlayer source)
        {
            // Navigator.Pop();
        }
        
        private void OnRedButtonClicked()
        {
            SFXManager.Play(R.Audios.指笛);
        }

        private void OnBlueButtonClicked()
        {
            SFXManager.Play(R.Audios.拍手);
        }

        private void OnGreenButtonClicked()
        {
            SFXManager.Play(R.Audios.歓声, 1.5f);
        }
        
        public AsyncOperationHandle<AudioClip>? PreloadAudio()
        {
            var index = RoulettePreferences.CommendationVideoOption;
            if (index < audioClips.Count)
                return Addressables.LoadAssetAsync<AudioClip>(audioClips[index]);
            return null;
        }
    }
}