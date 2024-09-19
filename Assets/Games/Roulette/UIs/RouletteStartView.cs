// Created by LunarEclipse on 2024-6-21 1:53.

using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Luna;
using Luna.UI;
using Luna.UI.Audio;
using Luna.UI.Navigation;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using USEN.Games.Common;

namespace USEN.Games.Roulette
{
    public class RouletteStartView : Widget
    {
        public Button startButton;
        public Button settingsButton;
        
        public AudioClip bgmClip;
        
        private RouletteDAO _dao;
        private RouletteDataset _dataset;
        
        private void Start()
        {
            EventSystem.current.SetSelectedGameObject(startButton.gameObject);
            BgmManager.Play(bgmClip);
            
            startButton.interactable = false;
            
            // Preload all roulette widgets
            // Widget.Load(GetType().Namespace);
            
            // Load the roulette data
            RouletteDAO.Instance.ContinueWith(async task => {
                _dao = task.Result;
                _dataset = _dao.Data;
                startButton.interactable = true;
                EventSystem.current.SetSelectedGameObject(startButton.gameObject);
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) ||
                Input.GetButtonDown("Cancel")) {
                OnExitButtonClicked();
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                BgmManager.Play(bgmClip);
            }
        }

        private void OnDestroy()
        {
            BgmManager.Stop();
            
            // Unload all roulette widgets
            // Widget.Unload(GetType().Namespace);
        }

        public void OnStartButtonClicked()
        {
            switch (RoulettePreferences.DisplayMode)
            {
                case RouletteDisplayMode.Normal:
                    Navigator.Push<RouletteCategoryView>((view) => {
                        view.Categories = _dataset.categories;
                    });
                    break;
                case RouletteDisplayMode.Random:
                    PlayRandomGame();
                    break;
            }
        }

        public void PlayRandomGame()
        {
            var category = _dataset.categories.First(); //[Random.Range(0, _dataset.categories.Count)];
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
            SceneManager.LoadScene("GameEntries");
        }
    }
}