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
            listView.FocusOnCell(0);
            
            if (Categories.IsNullOrEmpty())
            {
                RouletteDAO.Instance.ContinueWith(task =>
                {
                    Categories = task.Result.Data.categories;
                    listView.FocusOnCell(0);
                });
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
