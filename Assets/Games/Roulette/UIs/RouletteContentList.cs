// Created by LunarEclipse on 2024-7-6 21:4.

using Luna;
using Luna.UI;
using UnityEngine;

namespace USEN.Games.Roulette
{
    public class RouletteContentList : FixedListView<RouletteContentListCell, RouletteSector>
    {
        protected override void OnCellSubmitted(int index, RouletteContentListCell listViewCell)
        {
            SFXManager.Play(R.Audios.SfxRouletteConfirm);
        }

        protected override void OnCellSelected(int index, RouletteContentListCell listViewCell)
        {
            if (Initialized)
                SFXManager.Play(R.Audios.SfxRouletteSelect);
        }
    }
}