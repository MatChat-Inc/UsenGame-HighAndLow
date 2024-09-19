using LeTai;
using Luna.Extensions.Unity;
using Luna.UI.Navigation;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace USEN.Games.Roulette
{
    public class RouletteContentListCell : FixedListViewCell<RouletteSector>
    {
        public TextMeshProUGUI text;
    
        private RouletteSector _data;
        public override RouletteSector Data
        {
            get => _data;
            set
            {
                _data = value;
                if (text != null)
                    text.text = value.content;
                
                colors = new ColorBlock
                {
                    normalColor = value.color.With(0.7f, 0.7f).WithAlpha(0.8f),
                    highlightedColor =  value.color.With(1f),
                    pressedColor =  value.color.With(1f).WithAlpha(0.8f),
                    selectedColor = value.color.With(1f),
                    disabledColor = colors.disabledColor,
                    colorMultiplier = colors.colorMultiplier,
                    fadeDuration = colors.fadeDuration
                };
            }
        }

        void Awake()
        {
        }
        
        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            
            text.color = Color.black;
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);
            
            text.color = Color.white;
        }
    }
}
