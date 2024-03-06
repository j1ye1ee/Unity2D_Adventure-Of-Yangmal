using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorySceneAudio : MonoBehaviour
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
        StartCoroutine(GetComponent<AudioFade>().FadeIn(_backMusicSource));
    }

    // 버튼 셀렉트 & submit 이벤트 트리거
    public void SelectButton()
    {
        _buttonSource.PlayOneShot(_buttonSource.clip);
    }

    //  스킵 버튼 셀렉트시 배경음 페이드아웃
    public void FadeOut()
    {
        AudioFade audio = GetComponent<AudioFade>();
        StartCoroutine(audio.FadeOut(_backMusicSource));
    }
        
}
