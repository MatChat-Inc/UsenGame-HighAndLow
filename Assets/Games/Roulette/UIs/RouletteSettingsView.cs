// Created by LunarEclipse on 2024-7-18 9:26.

using System;
using Luna.UI;
using Luna.UI.Navigation;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using USEN.Games.Common;
using ToggleGroup = Modules.UI.ToggleGroup;

namespace USEN.Games.Roulette
{
    public class RouletteSettingsView: Widget
    {
        public Slider basicDisplayShowSettingsSlider;
        public ToggleGroup basicDisplaySettingsToggles;
        public Button appInfoButton;
        public BottomPanel bottomPanel;

        private void Start()
        {
            // Set the slider value to the current display mode
            var selectedIndex = (int) RoulettePreferences.DisplayMode;
            basicDisplayShowSettingsSlider.maxValue = basicDisplaySettingsToggles.Toggles.Count - 1;
            basicDisplayShowSettingsSlider.value = selectedIndex;
            basicDisplayShowSettingsSlider.onValueChanged.AddListener(OnBasicDisplayShowSettingsSliderValueChanged);
            
            basicDisplaySettingsToggles.ToggleOn(selectedIndex);
            foreach (var toggle in basicDisplaySettingsToggles.Toggles)
                toggle.onValueChanged.AddListener((isOn) => {
                    if (isOn)
                    {   // Clicking on the toggle will change the slider value
                        var index = basicDisplaySettingsToggles.Toggles.IndexOf(toggle);
                        basicDisplayShowSettingsSlider.value = index;
                        RoulettePreferences.DisplayMode = (RouletteDisplayMode) index;
                    }
                });
            
            appInfoButton.onClick.AddListener(OnClickAppInfoButton);
            bottomPanel.exitButton.onClick.AddListener(() => Navigator.Pop());
        }

        private void OnBasicDisplayShowSettingsSliderValueChanged(float arg0)
        {
            var index = Convert.ToInt32(arg0);
            basicDisplaySettingsToggles.ToggleOn(Convert.ToInt32(index));
            RoulettePreferences.DisplayMode = (RouletteDisplayMode) index;
        }

        private void OnEnable()
        {
            EventSystem.current.SetSelectedGameObject(basicDisplayShowSettingsSlider.gameObject);
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) ||
                Input.GetButtonDown("Cancel")) 
                Navigator.Pop();
        }
        
        void OnClickAppInfoButton() 
        {
            Navigator.Push<AppInfoView>();
        }
    }
}