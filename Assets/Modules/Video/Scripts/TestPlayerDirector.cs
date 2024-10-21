// Created by LunarEclipse on 2024-5-31 22:54.

using System;
using UnityEngine;
using UnityEngine.Playables;

namespace HightAndLowGame
{
    public class TestPlayerDirector : MonoBehaviour
    {
        private PlayableDirector _playableDirector;
        public PlayableAsset timelineAsset1; 
        public PlayableAsset timelineAsset2; 
        
        void Awake()
        {
            _playableDirector = GetComponent<PlayableDirector>();
        }

        private void Start()
        {
            
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                _playableDirector.Play(timelineAsset1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                _playableDirector.Play(timelineAsset2);
            }
        }
    }
}