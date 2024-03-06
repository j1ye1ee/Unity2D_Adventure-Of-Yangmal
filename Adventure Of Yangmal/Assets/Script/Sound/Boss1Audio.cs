using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Audio : MonoBehaviour
{
    public AudioClip _backMusic;
    public AudioSource backMusic;

    private void Awake()
    {
        backMusic.clip = _backMusic;

        backMusic.Play();
        StartCoroutine(GetComponent<AudioFade>().FadeIn(backMusic));
    }
}
