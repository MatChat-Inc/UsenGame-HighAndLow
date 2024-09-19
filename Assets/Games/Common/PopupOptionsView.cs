// Created by LunarEclipse on 2024-8-8 11:46.

using Luna.UI;
using UnityEngine;

namespace USEN.Games.Common
{
    public class PopupOptionsView : Widget
    {
        public delegate void OnOptionClicked();
        
        public OnOptionClicked onOption1;
        public OnOptionClicked onOption2;
        public OnOptionClicked onOption3;
        
        public void OnOption1()
        {
            onOption1?.Invoke();
        }
        
        public void OnOption2()
        {
            onOption2?.Invoke();
        }
        
        public void OnOption3()
        {
            onOption3?.Invoke();
        }
    }
}