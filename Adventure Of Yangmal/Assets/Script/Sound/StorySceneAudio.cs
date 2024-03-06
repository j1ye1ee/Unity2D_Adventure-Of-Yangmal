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

    // ��ư ����Ʈ & submit �̺�Ʈ Ʈ����
    public void SelectButton()
    {
        _buttonSource.PlayOneShot(_buttonSource.clip);
    }

    //  ��ŵ ��ư ����Ʈ�� ����� ���̵�ƿ�
    public void FadeOut()
    {
        AudioFade audio = GetComponent<AudioFade>();
        StartCoroutine(audio.FadeOut(_backMusicSource));
    }
        
}
