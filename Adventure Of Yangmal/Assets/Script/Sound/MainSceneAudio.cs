using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneAudio : MonoBehaviour
{
    public AudioClip _button;
    public AudioClip _backMusic;

    public AudioSource _buttonSource;
    public AudioSource _backMusicSource;

    private void Awake()
    {
        _buttonSource.clip = _button;
        _backMusicSource.clip = _backMusic;

        _backMusicSource.Play();
    }
}
