using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioClip musicGame;

    AudioSource musicSource;
    // Start is called before the first frame update
    private void Awake()
    {
        musicSource = gameObject.AddComponent<AudioSource>();

        
    }

    private void Start()
    {
        musicSource.clip = musicGame;
        musicSource.volume = 0.8f;
        musicSource.loop = true;
        musicSource.Play();
    }
}
