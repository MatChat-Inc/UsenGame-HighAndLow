// Created by LunarEclipse on 2024-7-6 21:58.

using System;
using Luna;
using Luna.UI.Navigation;
using UnityEngine;
using UnityEngine.UI;

namespace USEN.Games.Common
{
    public class BottomPanel: MonoBehaviour
    {
        public Button exitButton;
        public Button selectButton;
        public Button confirmButton;
        public Button redButton;
        public Button blueButton;
        public Button greenButton;
        public Button yellowButton;
        
        public bool clickExitToPop = false;
        public bool playSound = true;
        
        public event Action onExitButtonClicked;
        public event Action onSelectButtonClicked;
        public event Action onConfirmButtonClicked;
        public event Action onRedButtonClicked;
        public event Action onBlueButtonClicked;
        public event Action onGreenButtonClicked;
        public event Action onYellowButtonClicked;
        
        private EventHandler<EventArgs> _redButtonEventHandler;
        private EventHandler<EventArgs> _blueButtonEventHandler;
        private EventHandler<EventArgs> _greenButtonEventHandler;
        private EventHandler<EventArgs> _yellowButtonEventHandler;
        
        void Awake()
        {
            exitButton.onClick.AddListener(OnExitButtonClicked);
            selectButton.onClick.AddListener(OnSelectButtonClicked);
            confirmButton.onClick.AddListener(OnConfirmButtonClicked);
            redButton.onClick.AddListener(OnRedButtonClicked);
            blueButton.onClick.AddListener(OnBlueButtonClicked);
            greenButton.onClick.AddListener(OnGreenButtonClicked);
            yellowButton.onClick.AddListener(OnYellowButtonClicked);
            
            _redButtonEventHandler = (sender, args) => OnRedButtonClicked();
            _blueButtonEventHandler = (sender, args) => OnBlueButtonClicked();
            _greenButtonEventHandler = (sender, args) => OnGreenButtonClicked();
            _yellowButtonEventHandler = (sender, args) => OnYellowButtonClicked();
        }

        private void OnEnable()
        {
            UsenEvents.OnRemoconRedButtonClicked += _redButtonEventHandler;
            UsenEvents.OnRemoconBlueButtonClicked += _blueButtonEventHandler;
            UsenEvents.OnRemoconGreenButtonClicked += _greenButtonEventHandler;
            UsenEvents.OnRemoconYellowButtonClicked += _yellowButtonEventHandler;
        }
        
        private void OnDisable()
        {
            UsenEvents.OnRemoconRedButtonClicked -= _redButtonEventHandler;
            UsenEvents.OnRemoconBlueButtonClicked -= _blueButtonEventHandler;
            UsenEvents.OnRemoconGreenButtonClicked -= _greenButtonEventHandler;
            UsenEvents.OnRemoconYellowButtonClicked -= _yellowButtonEventHandler;
        }

        private void Update()
        {
#if DEBUG
            if (Input.GetKeyDown(KeyCode.Keypad0))
                OnExitButtonClicked();
            else if (Input.GetKeyDown(KeyCode.KeypadPeriod))
                OnSelectButtonClicked();
            else if (Input.GetKeyDown(KeyCode.KeypadEnter))
                OnConfirmButtonClicked();
            
            if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1))
                OnBlueButtonClicked();
            else if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2))
                OnRedButtonClicked();
            else if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3))
                OnGreenButtonClicked();
            else if (Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Alpha4))
                OnYellowButtonClicked();
#endif
        }

        private void OnExitButtonClicked()
        {
            if (clickExitToPop)
                Navigator.Pop();
            
            if (!exitButton.isActiveAndEnabled) return;
            onExitButtonClicked?.Invoke();
            // SFXManager.Play(R.Audios.ルーレット操作音戻る);
        }

        private void OnSelectButtonClicked()
        {
            if (!selectButton.isActiveAndEnabled) return;
            onSelectButtonClicked?.Invoke();
        }

        private void OnConfirmButtonClicked()
        {
            if (!confirmButton.isActiveAndEnabled) return;
            onConfirmButtonClicked?.Invoke();
            
            if (playSound)
                SFXManager.Play(R.Audios.SfxRouletteConfirm);
        }

        private void OnRedButtonClicked()
        {
            if (!redButton.isActiveAndEnabled) return;
            onRedButtonClicked?.Invoke();
            
            if (playSound)
                SFXManager.Play(R.Audios.SfxRouletteConfirm);
        }

        private void OnBlueButtonClicked()
        {
            if (!blueButton.isActiveAndEnabled) return;
            onBlueButtonClicked?.Invoke();
            if (playSound)
                SFXManager.Play(R.Audios.SfxRouletteConfirm);
        }

        private void OnGreenButtonClicked()
        {
            if (!greenButton.isActiveAndEnabled) return;
            onGreenButtonClicked?.Invoke();
            
            if (playSound)
                SFXManager.Play(R.Audios.SfxRouletteConfirm);
        }

        private void OnYellowButtonClicked()
        {
            if (!yellowButton.isActiveAndEnabled) return;
            onYellowButtonClicked?.Invoke();
            
            if (playSound)
                SFXManager.Play(R.Audios.SfxRouletteConfirm);
        }
    }
}