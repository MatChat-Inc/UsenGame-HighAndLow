// Created by LunarEclipse on 2024-7-18 9:26.

using System;
using Luna;
using Luna.UI;
using Luna.UI.Navigation;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using USEN.Games.Common;
using ToggleGroup = Modules.UI.ToggleGroup;

namespace USEN.Games.Roulette
{
    public class RouletteSettingsView: Widget
    {
        public ToggleGroup basicDisplaySettingsToggles;
        public Slider basicDisplayShowSettingsSlider;
        public ToggleGroup commendationVideoSettingsToggles;
        public Slider commendationVideoSettingsSlider;
        public Slider bgmVolumeSlider;
        public Slider sfxVolumeSlider;
        public Text bgmVolumeText;
        public Text sfxVolumeText;
        public Button appInfoButton;
        public BottomPanel bottomPanel;

        private void Start()
        {
            // Current display mode
            var selectedIndex = (int) RoulettePreferences.DisplayMode;
            basicDisplayShowSettingsSlider.maxValue = basicDisplaySettingsToggles.Toggles.Count - 1;
            basicDisplayShowSettingsSlider.value = selectedIndex;
            basicDisplayShowSettingsSlider.onValueChanged.AddListener(OnBasicDisplayShowSettingsSliderValueChanged);
            
            basicDisplaySettingsToggles.ToggleOn(selectedIndex);
            basicDisplaySettingsToggles.Bind(basicDisplayShowSettingsSlider);
            
            // Commendation video settings
            commendationVideoSettingsSlider.onValueChanged.AddListener(OnCommendationVideoSettingsSliderValueChanged);
            commendationVideoSettingsSlider.value = RoulettePreferences.CommendationVideoOption;
            commendationVideoSettingsToggles.ToggleOn(RoulettePreferences.CommendationVideoOption);
            
            // Audio volume
            bgmVolumeText.text = $"{RoulettePreferences.BgmVolume * 100:0}";
            bgmVolumeSlider.value = BgmManager.Volume * 10;
            bgmVolumeSlider.onValueChanged.AddListener(OnBgmVolumeChanged);
            
            sfxVolumeText.text = $"{RoulettePreferences.SfxVolume * 100:0}";
            sfxVolumeSlider.value = SFXManager.Volume * 10;
            sfxVolumeSlider.onValueChanged.AddListener(OnSfxVolumeChanged);
            
            // App info
            appInfoButton.onClick.AddListener(OnClickAppInfoButton);
            
            // Bottom panel
            bottomPanel.exitButton.onClick.AddListener(() => Navigator.Pop());
            
            EventSystem.current.SetSelectedGameObject(basicDisplayShowSettingsSlider.gameObject);
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) ||
                Input.GetButtonDown("Cancel")) 
                Navigator.Pop();
        }
        
        private void OnBasicDisplayShowSettingsSliderValueChanged(float arg0)
        {
            var index = Convert.ToInt32(arg0);
            // basicDisplaySettingsToggles.ToggleOn(Convert.ToInt32(index));
            RoulettePreferences.DisplayMode = (RouletteDisplayMode) index;
            API.UpdateRandomSetting(RoulettePreferences.DisplayMode == RouletteDisplayMode.Random);
        }
        
        private void OnCommendationVideoSettingsSliderValueChanged(float arg0)
        {
            RoulettePreferences.CommendationVideoOption = Convert.ToInt32(arg0);
        }
        
        private void OnBgmVolumeChanged(float value)
        {
            BgmManager.SetVolume(value * 0.1f);
            RoulettePreferences.BgmVolume = value * 0.1f;
            bgmVolumeText.text = $"{value * 10:0}";
        }
        
        private void OnSfxVolumeChanged(float value)
        {
            SFXManager.SetVolume(value * 0.1f);
            RoulettePreferences.SfxVolume = value * 0.1f;
            sfxVolumeText.text = $"{value * 10:0}";
            SFXManager.Play(R.Audios.SfxRouletteBack);
        }
        
        private void OnClickAppInfoButton() 
        {
            Navigator.Push<AppInfoView>();
        }
    }
}