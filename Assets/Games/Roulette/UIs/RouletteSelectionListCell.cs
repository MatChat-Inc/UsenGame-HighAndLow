using Luna.UI.Navigation;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using USEN.Games.Roulette;

public class RouletteSelectionListCell : MonoBehaviour, ISelectHandler, IDeselectHandler, ISubmitHandler
{
    public TextMeshProUGUI text;
    
    public void OnSelect(BaseEventData eventData)
    {
        text.color = Color.black;
        
        // Emit event
        ExecuteEvents.ExecuteHierarchy<RouletteSelectionList>(gameObject, null, (target, data) =>
        {
            target.SnapTo(transform as RectTransform);
        });
    }

    public void OnDeselect(BaseEventData eventData)
    {
        text.color = Color.white;
    }

    public void OnSubmit(BaseEventData eventData)
    {
        // Navigator.Push<RouletteGameSelectionView>();
    }
    
    public void OnClick()
    {
        Navigator.Push<RouletteGameSelectionView>();
    }
    
    public void Test()
    {
        Debug.Log("Test");
    }
}


