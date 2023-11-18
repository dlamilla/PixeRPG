using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class CloseVideo : MonoBehaviour
{
    private VideoPlayer video;
    // Start is called before the first frame update
    private void Awake()
    {
        video = GetComponent<VideoPlayer>();
        video.Play();
        video.loopPointReached += CheckOver;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CheckOver(VideoPlayer vd)
    {
        SceneManager.LoadScene(2);
    }
}
