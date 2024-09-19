using System.Linq;
using Luna.UI.Navigation;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using USEN.Games.Roulette;

public class RouletteCategoryListCell : FixedListViewCell<RouletteCategory>, ISelectHandler, IDeselectHandler, ISubmitHandler
{
    public TextMeshProUGUI text;
    
    private RouletteCategory _rouletteCategory;
    
    public override RouletteCategory Data
    {
        get => _rouletteCategory;
        set
        {
            _rouletteCategory = value;
            text.text = value.title;
        }
    }
}


