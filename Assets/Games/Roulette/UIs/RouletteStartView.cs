// Created by LunarEclipse on 2024-6-21 1:53.

using System;
using System.Linq;
using System.Threading.Tasks;
using Luna;
using Luna.UI;
using Luna.UI.Navigation;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace USEN.Games.Roulette
{
    public class RouletteStartView : Widget
    {
        public Button startButton;
        public Button settingsButton;
        
        public AudioClip bgmClip;
        
        private RouletteManager _manager;
        private RouletteCategories _categories;
        
        private void Start()
        {
            BgmManager.Play(bgmClip);
            
            // Preload all roulette widgets
            Assets.Load<Object>(GetType().Namespace, "Audio");
            
            // Audio volume
            BgmManager.Volume = RoulettePreferences.BgmVolume;
            SFXManager.Volume = RoulettePreferences.SfxVolume;
            
            Navigator.Instance.onPopped += (route) => {
                SFXManager.Play(R.Audios.SfxRouletteBack);
            };

#if UNITY_ANDROID
            Debug.Log($"TV: {USEN.AndroidPreferences.TVIdentifier}");      
#endif
        }

        private void OnEnable()
        {
            // Load the roulette data
            RouletteManager.Instance.Sync().ContinueWith(async task => {
                _categories = task.Result;
                if (startButton.interactable == false)
                    EventSystem.current.SetSelectedGameObject(startButton.gameObject);
                startButton.interactable = true;
            }, TaskScheduler.FromCurrentSynchronizationContext());
            
            API.GetRandomSetting().ContinueWith(task => {
                RoulettePreferences.DisplayMode = (RouletteDisplayMode) task.Result.random;
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) ||
                Input.GetButtonDown("Cancel")) {
                OnExitButtonClicked();
            }
#if UNITY_ANDROID
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                AndroidPreferences.Toast("Hello, Kotlin!");
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Luna.Android.ShowToast("Hello, Android!");
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                Luna.Android.ShowToast(USEN.AndroidPreferences.TVIdentifier);
            }
#endif
        }

        private void OnDestroy()
        {
            BgmManager.Stop();
            
            // Unload all roulette widgets
            // Widget.Unload(GetType().Namespace);
        }

        public void OnStartButtonClicked()
        {
            Navigator.Push<RouletteCategoryView>((view) => {
                view.Categories = _categories.categories;
            });
        }

        public void PlayRandomGame()
        {
            var category = _categories.categories.First(); //[Random.Range(0, _dataset.categories.Count)];
            var rouletteData = category.roulettes[Random.Range(0, category.roulettes.Count)];
            Navigator.Push<RouletteGameView>((view) => {
                view.RouletteData = rouletteData;
            });
        }

        public void OnSettingsButtonClicked()
        {
            Navigator.Push<RouletteSettingsView>();
        }
        
        public void OnExitButtonClicked()
        {
            // Application.Quit();
#if UNITY_ANDROID
            Android.Back();
#endif
        }
    }
}