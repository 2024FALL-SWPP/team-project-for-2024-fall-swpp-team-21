using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VidPlayerForWebGL : MonoBehaviour
{
    public string videoName;

    private void Start()
    {
        StartVideo();
    }

    private void StartVideo()
    {
        VideoPlayer videoPlayer = gameObject.GetComponent<VideoPlayer>();
        if (videoPlayer != null)
        {
            string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, videoName);
            Debug.Log("Video Path: " + videoPath);
            videoPlayer.url = videoPath;
            videoPlayer.Play();
        }
    }
}
