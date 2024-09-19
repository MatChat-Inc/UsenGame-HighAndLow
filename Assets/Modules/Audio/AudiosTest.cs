using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Luna.UI.Audio;
using Luna.UI.Audio.Luna.UI;
using Modules.UI.Misc;
using UnityEngine;

public class AudiosTest : MonoBehaviour
{
    public Audios audios;
    
    void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (audios == null)
            {
                Debug.LogWarning("Audios is null. Loading from Resources...");
                audios = Resources.Load<Audios>("Audios.g");
            }
            SFXManager.Play(audios.Clips.GetRandomly());
        }
    }
}
