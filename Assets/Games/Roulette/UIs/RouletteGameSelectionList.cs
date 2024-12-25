using Luna;
using Luna.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace USEN.Games.Roulette
{
    
    public class RouletteGameSelectionList : FixedListView<RouletteGameSelectionListCell, RouletteData>, IEventSystemHandler
    {
        protected override void OnCellSubmitted(int index, RouletteGameSelectionListCell listViewCell)
        {
            SFXManager.Play(R.Audios.SfxConfirm);
        }

        protected override void OnCellDeselected(int index, RouletteGameSelectionListCell listViewCell)
        {
            listViewCell.text.color = Color.white;
        }
        
        protected override void OnCellSelected(int index, RouletteGameSelectionListCell listViewCell)
        {
            listViewCell.text.color = Color.black;
            
            if (Initialized)
                SFXManager.Play(R.Audios.SfxSelect);
        }
    }

}
