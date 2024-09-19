// Created by LunarEclipse on 2024-7-12 22:2.

using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace USEN.Games.Roulette
{
    public class RouletteEditListCell : FixedListViewCell<RouletteSector>
    {
        public TextMeshProUGUI indexText;
        public TMP_InputField inputField;
        
        public event Action<int, RouletteEditListCell, string> onInputValueChanged;
        public event Action<int, RouletteEditListCell, string> onInputEnd;
        
        private RouletteSector _data;
        
        public override RouletteSector Data
        {
            get => _data;
            set
            {
                _data = value;
                inputField.text = value.content;
            }
        }

        void Start()
        {
            if (indexText != null)
                indexText.text = (Index + 1).ToString();

            inputField.onValueChanged.AddListener(OnInputValueChanged);
            inputField.onEndEdit.AddListener(OnInputEnd);
            
            // this.colors = new ColorBlock
            // {
            //     normalColor = Color.clear,
            //     highlightedColor = new Color(0.8f, 0.8f, 0.8f, 0.3f),
            //     pressedColor = new Color(0.6f, 0.6f, 0.6f, 0.3f),
            //     selectedColor = new Color(0.8f, 0.8f, 0.8f, 0.3f),
            //     disabledColor = new Color(0f, 0f, 0f, 0.3f),
            //     colorMultiplier = 1,
            //     fadeDuration = 0.1f
            // };
        }

        private void OnInputEnd(string value)
        {
            this.Focus();
            onInputEnd?.Invoke(Index, this, value);
        }

        private void OnInputValueChanged(string newValue)
        {
            onInputValueChanged?.Invoke(Index, this, newValue);
        }

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            Debug.Log($"[RouletteEditListCell] OnSelect: {Index}");
            inputField.DeactivateInputField();
        }
        
        public override async void OnSubmit(BaseEventData eventData)
        {
            base.OnSubmit(eventData);
            Debug.Log($"[RouletteEditListCell] OnSubmit: {Index}");
            
            await UniTask.NextFrame();
            inputField.Select();
        }
    }
}