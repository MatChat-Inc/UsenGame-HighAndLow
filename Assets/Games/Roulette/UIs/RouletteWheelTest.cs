// Created by LunarEclipse on 2024-6-5 9:22.

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace USEN.Games.Roulette
{
    public class RouletteWheelTest : MonoBehaviour
    {
        public RouletteWheel rouletteWheel;
        public RouletteData rouletteData;
        public TextMeshProUGUI resultText;
        
        private List<RouletteSector> sectors;

        private void Start()
        {
            sectors = new List<RouletteSector>(rouletteData.sectors);
            rouletteWheel.OnSpinEnd += result =>
            {
                resultText.text = result;
            };
        }
                
        void Update()
        {
            // if (Input.GetKeyDown(KeyCode.Escape) ||
            //     Input.GetButtonDown("Cancel")) {
            //     SceneManager.LoadScene("GameEntries");
            // }
            
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                rouletteWheel.Spin();
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                // Duplicate random sector
                var randomIndex = Random.Range(0, sectors.Count);
                sectors.Add(new RouletteSector(sectors[randomIndex]));
                // rouletteWheel.Sectors = sectors;
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                // Remove last sector
                sectors.RemoveAt(sectors.Count - 1);
                // rouletteWheel.Sectors = sectors;
            }
        }

        public void OnAndroidKeyDown(string keyName)
        {
            if (keyName == "blue")
            {
                // Duplicate random sector
                var randomIndex = Random.Range(0, sectors.Count);
                sectors.Add(new RouletteSector(sectors[randomIndex]));
                // rouletteWheel.Sectors = sectors;
            }

            if (keyName == "red")
            {
                // Remove last sector
                sectors.RemoveAt(sectors.Count - 1);
                // rouletteWheel.Sectors = sectors;
            }

            if (keyName == "yellow")
            {
                Debug.Log("Yellow key pressed");
                rouletteWheel.Spin();
            }

            if (keyName == "green")
            {
                Debug.Log("Green key pressed");
            }
        }

    }
}