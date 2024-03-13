using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndStageUi : MonoBehaviour
{

    public GameObject _backGround;
    public GameObject _textBox;
    public GameObject _mainText;
    public Text _subText;
    public GameObject _button;
    public GameObject _goToNext;

    public AudioSource _buttonSound;
    public Button _firstButton;




    public IEnumerator Flow()
    {
        // 플레이어 정지
        UiManager.Instance.StopPlayer(PlayerStatus.Instance.gameObject);

        // 남은 공격에 플레이어가 당하지 않도록 처리
        if(PlayerStatus.Instance != null)
            PlayerStatus.Instance.gameObject.GetComponent<Collider2D>().enabled = false;

        // 배경 페이드인
        if (_backGround != null)
        {
            _backGround.SetActive(true);
            StartCoroutine(_backGround.GetComponent<UiEffect>().FadeIn());

            yield return new WaitUntil(() => _backGround.GetComponent<UiEffect>()._isFadeInEnd == true);
            _backGround.GetComponent<UiEffect>()._isFadeInEnd = false;
        }


        // 텍스트상자 open
        if (_textBox != null)
        {
            _textBox.SetActive(true);
            StartCoroutine(_textBox.GetComponent<UiEffect>().Spread());

            yield return new WaitUntil(() => _textBox.GetComponent<UiEffect>()._isSpreadEnd == true);
            _textBox.GetComponent<UiEffect>()._isSpreadEnd = false;
        }



        // 텍스트 open
        if (_mainText != null)
        {
            _mainText.SetActive(true);

            if (_subText != null)
            {
                _subText.gameObject.SetActive(true);
                StartCoroutine(_subText.GetComponent<UiEffect>().TextPrint());
                yield return new WaitUntil(() => _subText.GetComponent<UiEffect>()._isPrintEnd == true);
                _subText.GetComponent<UiEffect>()._isPrintEnd = false;
            }

        }

        // 버튼 open
        if (_button != null)
        {
            _button.SetActive(true);
            if (_firstButton != null)
                _firstButton.Select();
        }

        // nextScene 이동 엔터 활성화
        if(_goToNext != null)
            _goToNext.SetActive(true);

    }

    public void ButtonSoundPlay()
    {
        _buttonSound.PlayOneShot(_buttonSound.clip);
    }
}
