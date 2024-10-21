// Created by LunarEclipse on 2024-5-30 18:7.

using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerRenderTexture : MonoBehaviour
{
    public RenderTexture renderTexture;
    public VideoPlayer videoPlayer;

    void Start()
    {
        // Ensure the video player and render texture are assigned
        if (videoPlayer == null || renderTexture == null)
        {
            Debug.LogError("VideoPlayer or RenderTexture is not assigned.");
            return;
        }

        ClearRenderTexture();

        // Register callback for when the video starts playing
        videoPlayer.prepareCompleted += OnVideoPrepared;
        videoPlayer.loopPointReached += OnVideoLoopPointReached;
        // videoPlayer.Prepare();
    }

    public void ClearRenderTexture()
    {
        RenderTexture.active = renderTexture;
        GL.Clear(true, true, Color.clear);
        RenderTexture.active = null;
    }

    void OnVideoPrepared(VideoPlayer vp)
    {
        Debug.Log("Video prepared.");
    }
    
    void OnVideoLoopPointReached(VideoPlayer vp)
    {
        Debug.Log("Video loop point reached.");
        
        // Clear the render texture when the video reaches the loop point
        // ClearRenderTexture();
    }
}
