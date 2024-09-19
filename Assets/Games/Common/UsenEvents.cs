// Created by LunarEclipse on 2024-7-14 10:1.

using System;
using Luna;

namespace USEN.Games.Common
{
    public class UsenEvents
    {
        public static Event<EventArgs> OnRemoconBlueButtonClicked = new();
        public static Event<EventArgs> OnRemoconGreenButtonClicked = new();
        public static Event<EventArgs> OnRemoconRedButtonClicked = new();
        public static Event<EventArgs> OnRemoconYellowButtonClicked = new();
        public static Event<EventArgs> OnRemoconTerminalButtonClicked = new();
    }
}