// Created by LunarEclipse on 2024-7-18 9:26.

using System;
using Luna.UI;
using Luna.UI.Navigation;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;
using USEN.Games.Common;

namespace USEN.Games.Roulette
{
    public class KeycodeTestView: Widget
    {
        public Button appInfoButton;
        public BottomPanel bottomPanel;
        
        public TextMeshProUGUI keyText;
        public TextMeshProUGUI inputText;
        public TextMeshProUGUI androidKeyText;

        private void Start()
        {
            appInfoButton.onClick.AddListener(OnClickAppInfoButton);
        }

        private void OnEnable()
        {
            base.OnKey += OnKey;
            base.OnInput += OnInput;
            EventSystem.current.SetSelectedGameObject(appInfoButton.gameObject);
            UsenEvents.OnRemoconButtonClicked += OnRemoconButtonClicked;
        }

        private void OnDisable()
        {
            base.OnKey -= OnKey;
            base.OnInput -= OnInput;
            UsenEvents.OnRemoconButtonClicked -= OnRemoconButtonClicked;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) 
                Navigator.Pop();
        }
        
        void OnClickAppInfoButton() 
        {
            
        }
        
        private void OnRemoconButtonClicked(object sender, string e)
        {
            // Debug.Log($"[KeyTest] Key pressed: {e}");
            androidKeyText.text = $"Android Key: {e}";
        }

        
        private KeyEventResult OnKey(KeyControl key, KeyEvent keyEvent)
        {
            Debug.Log($"[KeyTest] Key pressed: {key.keyCode}(0x{(int)key.keyCode:X8}) with event: {keyEvent}");
            keyText.text = $"Key: {key.keyCode} (0x{(int)key.keyCode:X8}) with event: {keyEvent}";
            return KeyEventResult.Unhandled;
        }
        
        private void OnInput(InputControl input, InputEvent @event)
        {
            Debug.Log($"[InputTest] Input: {input}({input.GetType()}) with event: {@event}");
            inputText.text = $"Input: {input} with event: {@event}";
            // text.text = $"Key: {input.keyCode} ({(int)input}) with event: {@event}";
        }
    }
}