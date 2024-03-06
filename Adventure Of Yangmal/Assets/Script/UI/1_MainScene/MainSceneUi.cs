using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainSceneUi : MonoBehaviour
{
    public GameObject _backGround;
    public GameObject _textBox;
    public GameObject _mainText;
    public Text _subText;
    public GameObject _button;
    public GameObject _goToNext;
    public Button _firstSelect;

    public GameObject _fadeOut;
    AudioSource _audio;
   

    private void Start()
    {
        _audio = null;
        StartCoroutine(Flow());
    }

    private void Update()
    {
        if (_audio != null)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
                _audio.Play();
        }
    }

    public IEnumerator Flow()
    {
        // 배경 페이드인

       _backGround.SetActive(true);
       StartCoroutine(_backGround.GetComponent<UiEffect>().FadeIn());

       yield return new WaitUntil(() => _backGround.GetComponent<UiEffect>()._isFadeInEnd == true);
        _backGround.GetComponent<UiEffect>()._isFadeInEnd = false;

        // 텍스트상자 open
        _textBox.SetActive(true);
        StartCoroutine(_textBox.GetComponent<UiEffect>().Spread());

        yield return new WaitUntil(() => _textBox.GetComponent<UiEffect>()._isSpreadEnd == true);
        _textBox.GetComponent<UiEffect>()._isSpreadEnd = false;


        // 텍스트 open
        _mainText.SetActive(true);


        // 서브텍스트 open
        _subText.gameObject.SetActive(true);
        StartCoroutine(_subText.GetComponent<UiEffect>().TextPrint());
        yield return new WaitUntil(() => _subText.GetComponent<UiEffect>()._isPrintEnd == true);
        _subText.GetComponent<UiEffect>()._isPrintEnd = false;


        // 버튼 open
        _button.SetActive(true);
        _firstSelect.Select();
        _audio = GameObject.FindWithTag("Audio").GetComponent<MainSceneAudio>()._buttonSource;
    }



    // ok
    public void OkButton()
    {
        _audio.Play();
        StartCoroutine(_audio.gameObject.GetComponent<AudioFade>().FadeOut(_audio));
        StartCoroutine(EnterToNextScene());
    }

    public void ExirBt()
    {
        _audio.Play();
        Application.Quit();
        Debug.Log("게임 종료");
    }

    IEnumerator EnterToNextScene()
    {
        _fadeOut.SetActive(true);
        StartCoroutine(_fadeOut.GetComponent<UiEffect>().FadeOut());
        yield return new WaitUntil(
            () => _fadeOut.GetComponent<UiEffect>()._isFadeOutEnd == true);


        // 페이드 아웃 완료 후 로드씬
        SceneManager.LoadScene("StoryScene");
    }
}
