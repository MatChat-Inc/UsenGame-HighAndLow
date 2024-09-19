// Created by LunarEclipse on 2024-6-3 9:18.

using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using UnityEngine;
using Sirenix.OdinInspector;

namespace USEN.Games.Roulette
{
    [Serializable]
    public class RouletteSector
    {
        [TableColumnWidth(150, Resizable = true)]
        [HideInInspector] public int id;
        [VerticalGroup("Data"), LabelWidth(50)] public string content;
        [VerticalGroup("Data"), LabelWidth(50)] public float weight = 1;
        [VerticalGroup("Data"), LabelWidth(80)] [InspectorName("Color")] [JsonIgnore] public Color color;
        
        [HideInInspector] 
        [JsonProperty("color")]
        public System.Drawing.Color scolor;
        
        public RouletteSector() {}

        public RouletteSector(int id, string content, float weight, Color color)
        {
            this.id = id;
            this.content = content;
            this.weight = weight;
            this.color = color;
        }
        
        public RouletteSector(RouletteSector sector)
        {
            id = sector.id;
            content = sector.content;
            weight = sector.weight;
            color = sector.color;
        }
        
        [OnSerializing]
        internal void OnSerializingMethod(StreamingContext context)
        {
            scolor = System.Drawing.Color.FromArgb((int)(color.r * 255), (int)(color.g * 255), (int)(color.b * 255));
        }
        
        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            color = new Color(scolor.R / 255f, scolor.G / 255f, scolor.B / 255f);
        }
    }
}