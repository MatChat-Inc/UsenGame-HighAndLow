using Luna.UI;
using Luna.UI.Navigation;
using UnityEngine;
using UnityEngine.EventSystems;

namespace USEN.Games.Roulette
{
    
    public class RouletteCategoryList : FixedListView<RouletteCategoryListCell, RouletteCategory>, IEventSystemHandler
    {
        protected override void OnCellClicked(int index, RouletteCategoryListCell listViewCell)
        {
            Navigator.Push<RouletteGameSelectionView>((view) =>
            {
                view.Category = SelectedData;
            });
        }
        
        protected override void OnCellSubmitted(int index, RouletteCategoryListCell listViewCell)
        {
            Navigator.Push<RouletteGameSelectionView>((view) =>
            {
                view.Category = SelectedData;
            });
        }

        protected override void OnCellDeselected(int index, RouletteCategoryListCell listViewCell)
        {
            listViewCell.text.color = Color.white;
        }

        protected override void OnCellSelected(int index, RouletteCategoryListCell listViewCell)
        {
            listViewCell.text.color = Color.black;
        }
    }

}
