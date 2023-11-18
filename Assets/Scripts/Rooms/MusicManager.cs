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

        musicSource.clip = musicGame;
    }

}
