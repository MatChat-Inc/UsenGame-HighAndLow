using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Luna.Core.Pool;
using Luna.UI;
using Luna.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace USEN.Games.Roulette
{
    
    public class RouletteGameSelectionList : FixedListView<RouletteGameSelectionListCell, RouletteData>, IEventSystemHandler
    {
        protected override void OnCellSubmitted(int index, RouletteGameSelectionListCell listViewCell)
        {
            
        }

        protected override void OnCellDeselected(int index, RouletteGameSelectionListCell listViewCell)
        {
            listViewCell.text.color = Color.white;
        }
        
        protected override void OnCellSelected(int index, RouletteGameSelectionListCell listViewCell)
        {
            listViewCell.text.color = Color.black;
        }
    }

}
