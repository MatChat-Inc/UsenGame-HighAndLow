using Luna;
using Luna.UI;
using Luna.UI.Navigation;
using Modules.UI.Misc;
using UnityEngine;
using UnityEngine.EventSystems;

namespace USEN.Games.Roulette
{
    
    public class RouletteCategoryList : FixedListView<RouletteCategoryListCell, RouletteCategory>, IEventSystemHandler
    {
        protected override void OnCellClicked(int index, RouletteCategoryListCell listViewCell)
        {
            SFXManager.Play(R.Audios.SfxRouletteConfirm);
            
            switch (RoulettePreferences.DisplayMode)
            {
                case RouletteDisplayMode.Normal:
                    Navigator.Push<RouletteGameSelectionView>((view) => {
                        view.Category = SelectedData;
                    });
                    break;
                case RouletteDisplayMode.Random:
                    if (SelectedData.roulettes.Count == 0)
                    {
                        Navigator.Push<RouletteGameSelectionView>((view) => {
                            view.Category = SelectedData;
                        });
                    }
                    else
                    {
                        Navigator.Push<RouletteGameView>((view) => {
                            view.RouletteData = SelectedData.roulettes.GetRandomly();
                        });
                    }

                    break;
            }
        }
        
        protected override void OnCellSubmitted(int index, RouletteCategoryListCell listViewCell)
        {
            OnCellClicked(index, listViewCell);
        }

        protected override void OnCellDeselected(int index, RouletteCategoryListCell listViewCell)
        {
            listViewCell.text.color = Color.white;
        }

        protected override void OnCellSelected(int index, RouletteCategoryListCell listViewCell)
        {
            listViewCell.text.color = Color.black;
            
            if (Initialized)
                SFXManager.Play(R.Audios.SfxRouletteSelect);
        }
    }

}
