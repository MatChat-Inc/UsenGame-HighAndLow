using System;
using System.Collections.Generic;
using Luna.UI;
using Luna.UI.Navigation;
using Sirenix.Utilities;
using UnityEngine;
using USEN.Games.Common;
using Random = UnityEngine.Random;

namespace USEN.Games.Roulette
{
    public class RouletteCategoryView : Widget
    {
        public RouletteCategoryList listView;
        public BottomPanel bottomPanel;

        public List<RouletteCategory> Categories
        {
            get => listView.Data;
            set => listView.Data = value;
        }

        void Start()
        {
            if (Categories.IsNullOrEmpty())
            {
                var categories = RouletteManager.Instance.GetCategories();
                if (!categories.IsNullOrEmpty())
                {
                    Categories = categories;
                    listView.FocusOnCell(0);
                }
                
                RouletteManager.Instance.Sync().ContinueWith(task =>
                {
                    Categories = RouletteManager.Instance.GetCategories();
                    listView.FocusOnCell(0);
                });
            }
        }

        private void OnEnable()
        {
            if (RouletteManager.Instance.IsDirty)
            {
                Categories = RouletteManager.Instance.GetCategories();
                listView.FocusOnCell(0);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) ||
                Input.GetButtonDown("Cancel")) {
                Navigator.Pop();
            }
        }
        public void GotoRandomCategory(Action<RouletteGameSelectionView> callback = null)
        {
            var randomIndex = Random.Range(0, Categories.Count);
            var category = Categories[randomIndex];
            Navigator.Push<RouletteGameSelectionView>((view) => {
                view.Category = category;
                callback?.Invoke(view);
            });
        }
        
        public void GotoOriginalCategory(Action<RouletteGameSelectionView> callback = null)
        {
            var category = Categories.Find(c => c.title == "オリジナル");
            Navigator.Push<RouletteGameSelectionView>((view) => {
                view.Category = category;
                callback?.Invoke(view);
            });
        }
    }
}
