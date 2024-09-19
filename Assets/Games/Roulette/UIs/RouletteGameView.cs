// Created by LunarEclipse on 2024-6-30 18:50.

using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Luna.UI;
using Luna.UI.Audio;
using Luna.UI.Navigation;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using USEN.Assets;
using USEN.Games.Common;

namespace USEN.Games.Roulette
{
    public class RouletteGameView : Widget
    {
        public RouletteWheel rouletteWheel;
        public Button startButton;
        public Image backgroundImage;
        public BottomPanel bottomPanel;
        public TextMeshProUGUI confirmText;

        private AsyncOperationHandle<AudioClip>? _audioClipHandle;

        public RouletteData RouletteData
        {
            get => rouletteWheel.RouletteData;
            set => rouletteWheel.RouletteData = value;
        }

        void OnEnable()
        {
            base.OnKey += OnKeyEvent;
            startButton.onClick.AddListener(OnStartButtonClicked);
            
            rouletteWheel.OnSpinStart += OnSpinStart;
            rouletteWheel.OnSpinEnd += OnSpinEnd;
            
            bottomPanel.onExitButtonClicked += OnExitButtonClicked;
            bottomPanel.onSelectButtonClicked += OnStartButtonClicked;
            bottomPanel.onConfirmButtonClicked += OnConfirmButtonClicked;
            bottomPanel.onRedButtonClicked += OnRedButtonClicked;
            bottomPanel.onBlueButtonClicked += OnBlueButtonClicked;
            bottomPanel.onYellowButtonClicked += OnYellowButtonClicked;
        }

        void OnDisable()
        {
            base.OnKey -= OnKeyEvent;
            startButton.onClick.RemoveListener(OnStartButtonClicked);
            
            rouletteWheel.OnSpinStart -= OnSpinStart;
            rouletteWheel.OnSpinEnd -= OnSpinEnd;
            
            bottomPanel.onExitButtonClicked -= OnExitButtonClicked;
            bottomPanel.onSelectButtonClicked -= OnStartButtonClicked;
            bottomPanel.onConfirmButtonClicked -= OnConfirmButtonClicked;
            bottomPanel.onRedButtonClicked -= OnRedButtonClicked;
            bottomPanel.onBlueButtonClicked -= OnBlueButtonClicked;
            bottomPanel.onYellowButtonClicked -= OnYellowButtonClicked;
        }

        private void Start()
        {
            EventSystem.current.SetSelectedGameObject(startButton.gameObject);
            AssetUtils.LoadAsync<CommendView>().ContinueWith(task =>
            {
                var go = task.Result;
                var commendView = go.GetComponent<CommendView>();
                if (commendView != null)
                    _audioClipHandle = commendView.PreloadAudio();
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) ||
                Input.GetButtonDown("Cancel"))
            {
                OnExitButtonClicked();
            }
        }

        private void OnDestroy()
        {
            AssetUtils.Unload<CommendView>();
            if (_audioClipHandle != null)
                Addressables.Release(_audioClipHandle.Value);
        }

        private KeyEventResult OnKeyEvent(KeyControl key, KeyEvent @event)
        {
            if (@event == KeyEvent.Down)
            {
                switch (key.keyCode)
                {
                    case Key.Enter:
                    case Key.Space:
                        OnStartButtonClicked();
                        break;
                }
            }

            return KeyEventResult.Unhandled;
        }

        private void OnStartButtonClicked()
        {
            if (rouletteWheel.IsSpinning)
                StopWheel();
            else SpinWheel();
        }

        private void OnConfirmButtonClicked()
        {
            if (rouletteWheel.IsSpinning)
                StopWheel();
            else SpinWheel();
        }

        private void OnExitButtonClicked()
        {
            PopupConfirmView();
        }

        private void OnBlueButtonClicked()
        {
            Navigator.Pop();
            
            if (Navigator.Instance.TopWidget is not RouletteGameSelectionView)
            {
                Navigator.Push<RouletteCategoryView>(async view => {
                    await UniTask.NextFrame();
                    view.GotoRandomCategory();
                });
            }
        }

        private void OnRedButtonClicked()
        {
            Navigator.Pop();
            Navigator.Pop();
            
            if (Navigator.Instance.TopWidget is not RouletteGameSelectionView)
            {
                Navigator.Push<RouletteCategoryView>();
            }
        }

        private async void OnYellowButtonClicked()
        {
            BgmManager.Pause();
            await Navigator.Push<CommendView>();
            BgmManager.Resume();
        }
        
        private void OnSpinStart()
        {
            confirmText.text = "停止";
            
            // Hide yellow button
            bottomPanel.yellowButton.gameObject.SetActive(false);
        }
        
        private void OnSpinEnd(string obj)
        {
            confirmText.text = "もう一度ルーレットを回す";
            
            // Show yellow button
            bottomPanel.yellowButton.gameObject.SetActive(true);
        }

        private async Task SpinWheel()
        {
            Debug.Log("Start button clicked.");

            // Hide buttons
            startButton.gameObject.SetActive(false);

            // Spin the wheel
            // rouletteWheel.SpinWheel();
            rouletteWheel.StartSpin();

            // Dotween move & scale
            // await UniTask.Delay((int)((rouletteWheel.spinDuration - 2) * 1000));
            // rouletteWheel.transform.parent.DOLocalMoveX(960, 1f).SetEase(Ease.InOutSine);
            // rouletteWheel.transform.parent.DOScale(3f, 1f).SetEase(Ease.InOutSine);
        }
        
        private async Task StopWheel()
        {
            Debug.Log("Stop button clicked.");

            // Stop the wheel
            rouletteWheel.StopSpin();

            // Dotween move & scale
            // await UniTask.Delay(1000);
            rouletteWheel.transform.parent.DOLocalMoveX(960, 1f).SetEase(Ease.InOutSine);
            rouletteWheel.transform.parent.DOScale(3f, 1f).SetEase(Ease.InOutSine);
        }

        private void PopupConfirmView()
        {
            Navigator.ShowModal<PopupOptionsView>(
                builder: (popup) =>
                {
                    popup.onOption1 = () => Navigator.Pop();
                    popup.onOption2 = () => Navigator.PopToRoot(); //Navigator.PopUntil<RouletteStartView>();
                    popup.onOption3 = () => SceneManager.LoadScene("GameEntries");
                });
        }
    }
}