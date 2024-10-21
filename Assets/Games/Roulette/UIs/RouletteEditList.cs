// Created by LunarEclipse on 2024-7-12 21:50.

using Luna;
using Luna.UI;
using UnityEngine;

namespace USEN.Games.Roulette
{
    public class RouletteEditList : FixedListView<RouletteEditListCell, RouletteSector>
    {
        protected override void OnCellSubmitted(int index, RouletteEditListCell listViewCell)
        {
            
        }

        protected override void OnCellDeselected(int index, RouletteEditListCell listViewCell)
        {
            
        }

        protected override void OnCellSelected(int index, RouletteEditListCell listViewCell)
        {
            SFXManager.Play(R.Audios.SfxRouletteSelect);
        }
    }
}