using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSceneAudio : MonoBehaviour
{

    public AudioClip _backMusic;
    public AudioSource backMusic;
    AudioListener _audio;


    private void Awake()
    {
        backMusic.clip = _backMusic;
        _audio = GameObject.FindWithTag("MainCamera").GetComponent<AudioListener>();
    }

    private void Start()
    {
        StartCoroutine(AudioDelay());
    }

    IEnumerator AudioDelay()
    {
        _audio.enabled = false;
        yield return new WaitForSeconds(2f);
        _audio.enabled = true;

        backMusic.Play();
        backMusic.loop = true;
        StartCoroutine(GetComponent<AudioFade>().FadeIn(backMusic));
    }
}
