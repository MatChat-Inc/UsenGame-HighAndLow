// Created by LunarEclipse on 2024-7-18 9:26.

using System;
using Luna.UI;
using Luna.UI.Navigation;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using USEN.Games.Common;

namespace USEN.Games.HighLow
{
    public class HighLowSettingsView: Widget
    {
        public Button highLowTimeButton;
        public Text highLowTimeCountText;
        
        public Button appInfoButton;
        public BottomPanel bottomPanel;

        private void Start()
        {
            appInfoButton.onClick.AddListener(OnClickAppInfoButton);
            highLowTimeCountText.text = AppConfig.Instance.CurrentHighAndLowTimer.ToString();
        }

        private void OnEnable()
        {
            EventSystem.current.SetSelectedGameObject(highLowTimeButton.gameObject);
        }

        private void OnDisable()
        {
            EventSystem.current.SetSelectedGameObject(null);
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
                    ChangeHighLowTimeValue(-5);
                else if (Input.GetKey(KeyCode.RightArrow)) 
                    ChangeHighLowTimeValue(+5);
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
        
        void OnClickAppInfoButton() 
        {
            Navigator.Push<AppInfoView>();
        }
    }
}