// Created by LunarEclipse on 2024-6-4 4:3.

using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;

namespace Luna
{
    public class RenderTextureInitializer : MonoBehaviour
    {
        public RenderTexture[] renderTextures;
        
        private void Start()
        {
#if UNITY_ANDROID
            var rts = renderTextures.IsNullOrEmpty() ? 
                Resources.FindObjectsOfTypeAll<RenderTexture>() : renderTextures;
            foreach (var rt in rts)
                ClearRenderTexture(rt);
#endif
        }
        
        public void ClearRenderTexture(RenderTexture rt)
        {
            RenderTexture.active = rt;
            GL.Clear(true, true, Color.clear);
            RenderTexture.active = null;
        }
    }
}