using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UnityEngine.UI.Image))]
public class SequenceSprite : MonoBehaviour
{
    [SerializeField] private Sprite[] allSprites;

    [SerializeField] private int interval;

    [SerializeField] private bool isLoop;

    private int totalInterval = 0;

    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
        if (interval <= 0)
        {
            interval = 1;
        }
    }

    private void OnEnable()
    {
        totalInterval = 0;
    }

    private void OnDisable()
    {
        image.sprite = allSprites[0];
    }

    private void Update()
    {
        if (totalInterval++ % interval == 0)
        {
            int index = totalInterval / interval;
            if (index >= allSprites.Length)
            {
                if (isLoop)
                {
                    index = index % allSprites.Length;
                    image.sprite = allSprites[index];
                }
                else
                {
                    //Destroy(this);
                }
            }
            else
            {
                image.sprite = allSprites[index];
            }
        }
    }
}