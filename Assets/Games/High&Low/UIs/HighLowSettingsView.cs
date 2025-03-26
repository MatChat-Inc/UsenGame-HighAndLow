// Created by LunarEclipse on 2024-7-18 9:26.

using System;
using Luna;
using Luna.UI;
using Luna.UI.Navigation;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using USEN.Games.Common;
using USEN.Games.Roulette;
using ToggleGroup = Modules.UI.ToggleGroup;

namespace USEN.Games.HighLow
{
    public class HighLowSettingsView: Widget
    {
        public ToggleGroup basicDisplaySettingsToggles;
        public Slider basicDisplayShowSettingsSlider;
        public ToggleGroup commendationVideoSettingsToggles;
        public Slider commendationVideoSettingsSlider;
        public Slider bgmVolumeSlider;
        public Slider sfxVolumeSlider;
        public Text bgmVolumeText;
        public Text sfxVolumeText;
        public Button highLowTimeButton;
        public Text highLowTimeCountText;
        
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
            commendationVideoSettingsToggles.Bind(commendationVideoSettingsSlider);
            
            // Audio volume
            bgmVolumeText.text = $"{AppConfig.Instance.BGMVolume * 10:0}";
            bgmVolumeSlider.value = AppConfig.Instance.BGMVolume;
            bgmVolumeSlider.onValueChanged.AddListener(OnBgmVolumeChanged);
            
            sfxVolumeText.text = $"{AppConfig.Instance.EffectVolume * 10:0}";
            sfxVolumeSlider.value = AppConfig.Instance.EffectVolume;
            sfxVolumeSlider.onValueChanged.AddListener(OnSfxVolumeChanged);
            
            highLowTimeCountText.text = AppConfig.Instance.CurrentHighAndLowTimer.ToString();
            
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
            
            if (Input.GetButtonDown("Horizontal") && 
                EventSystem.current.currentSelectedGameObject == highLowTimeButton.gameObject) 
            {
                if (Input.GetKey(KeyCode.LeftArrow)) 
                    ChangeHighLowTimeValue(-10);
                else if (Input.GetKey(KeyCode.RightArrow)) 
                    ChangeHighLowTimeValue(+10);
            }
        }
        
        public void ChangeHighLowTimeValue(int value) 
        {
            AppConfig.Instance.CurrentHighAndLowTimer += value;
            if (value < 0 && AppConfig.Instance.CurrentHighAndLowTimer < 10) 
                AppConfig.Instance.CurrentHighAndLowTimer = 10;
            if (value > 0 && AppConfig.Instance.CurrentHighAndLowTimer > 30) 
                AppConfig.Instance.CurrentHighAndLowTimer = 30;
            
            highLowTimeCountText.text = AppConfig.Instance.CurrentHighAndLowTimer.ToString();
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
            AppConfig.Instance.BGMVolume = Convert.ToInt32(value);
            AudioManager.Instance.SetBgmVolume((int)value);
            bgmVolumeText.text = $"{value * 10:0}";
        }
        
        private void OnSfxVolumeChanged(float value)
        {
            SFXManager.SetVolume(value * 0.1f);
            RoulettePreferences.SfxVolume = value * 0.1f;
            AppConfig.Instance.EffectVolume = Convert.ToInt32(value);
            AudioManager.Instance.SetEffectVolume((int)value);
            sfxVolumeText.text = $"{value * 10:0}";
            SFXManager.Play(R.Audios.SfxBack);
        }
        
        void OnClickAppInfoButton() 
        {
            Navigator.Push<AppInfoView>();
        }
    }
}