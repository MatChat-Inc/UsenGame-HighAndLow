// Created by LunarEclipse on 2024-7-14 10:6.

using System;
using UnityEngine;

namespace USEN.Games.Common
{
    public class UsenRemoteControlHandler : MonoBehaviour
    {
        public void OnAndroidKeyDown(string keyName) {
            switch (keyName)
            {
                case "red":
                    UsenEvents.OnRemoconRedButtonClicked.Invoke(this, EventArgs.Empty);
                    break;
                case "blue":
                    UsenEvents.OnRemoconBlueButtonClicked.Invoke(this, EventArgs.Empty);
                    break;
                case "green":
                    UsenEvents.OnRemoconGreenButtonClicked.Invoke(this, EventArgs.Empty);
                    break;
                case "yellow":
                    UsenEvents.OnRemoconYellowButtonClicked.Invoke(this, EventArgs.Empty);
                    break;
                case "terminal":
                    UsenEvents.OnRemoconTerminalButtonClicked.Invoke(this, EventArgs.Empty);
                    break;
            }
        }
    }
}