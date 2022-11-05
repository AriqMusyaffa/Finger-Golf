using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    public AudioSource EverAudio;
    public AudioClip menuMusic, ingameMusic;

    public void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        EverAudio = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
    }
}
