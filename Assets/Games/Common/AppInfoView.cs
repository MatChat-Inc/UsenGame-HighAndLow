// Created by LunarEclipse on 2024-7-8 16:42.

using Luna.UI;
using Luna.UI.Navigation;
using UnityEngine;
using UnityEngine.UI;

namespace USEN.Games.Common
{
    public class AppInfoView : Widget
    {
        public Text versionText;
        
        private void Start()
        {
            versionText.text = $"Version {Application.version}";
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) ||
                 Input.GetButtonDown("Cancel")) {
                Navigator.Pop();
            }
        }
    }
}