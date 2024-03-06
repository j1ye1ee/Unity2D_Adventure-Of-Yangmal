using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFade : MonoBehaviour
{
    float _originVolume;


    public IEnumerator FadeIn(AudioSource audio)
    {
        _originVolume = audio.volume;
        audio.volume = 0;

        audio.Play();

        while(audio.volume <= _originVolume)
        {
            audio.volume += 0.01f;
            yield return null;
        }
    }

    public IEnumerator FadeOut(AudioSource audio)
    {
        _originVolume = audio.volume;

        while (audio.volume >= 0)
        {
            audio.volume -= 0.01f;
            yield return null;
        }
    }
}
