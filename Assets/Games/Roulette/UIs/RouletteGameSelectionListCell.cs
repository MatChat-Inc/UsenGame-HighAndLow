using System.Linq;
using Luna.UI.Navigation;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using USEN.Games.Roulette;

public class RouletteGameSelectionListCell : FixedListViewCell<RouletteData>, ISelectHandler, IDeselectHandler, ISubmitHandler
{
    public RouletteWheel rouletteWheel;
    public TextMeshProUGUI text;
    
    private RouletteData _rouletteData;
    public override RouletteData Data
    {
        get => _rouletteData;
        set
        {
            _rouletteData = value;
            text.text = value.title;
        }
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


