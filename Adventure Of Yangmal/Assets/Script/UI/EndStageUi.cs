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
        // �÷��̾� ����
        UiManager.Instance.StopPlayer(PlayerStatus.Instance.gameObject);

        // ���� ���ݿ� �÷��̾ ������ �ʵ��� ó��
        if(PlayerStatus.Instance != null)
            PlayerStatus.Instance.gameObject.GetComponent<Collider2D>().enabled = false;

        // ��� ���̵���
        if (_backGround != null)
        {
            _backGround.SetActive(true);
            StartCoroutine(_backGround.GetComponent<UiEffect>().FadeIn());

            yield return new WaitUntil(() => _backGround.GetComponent<UiEffect>()._isFadeInEnd == true);
            _backGround.GetComponent<UiEffect>()._isFadeInEnd = false;
        }


        // �ؽ�Ʈ���� open
        if (_textBox != null)
        {
            _textBox.SetActive(true);
            StartCoroutine(_textBox.GetComponent<UiEffect>().Spread());

            yield return new WaitUntil(() => _textBox.GetComponent<UiEffect>()._isSpreadEnd == true);
            _textBox.GetComponent<UiEffect>()._isSpreadEnd = false;
        }



        // �ؽ�Ʈ open
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

        // ��ư open
        if (_button != null)
        {
            _button.SetActive(true);
            if (_firstButton != null)
                _firstButton.Select();
        }

        // nextScene �̵� ���� Ȱ��ȭ
        if(_goToNext != null)
            _goToNext.SetActive(true);

    }

    public void ButtonSoundPlay()
    {
        _buttonSound.PlayOneShot(_buttonSound.clip);
    }
}
