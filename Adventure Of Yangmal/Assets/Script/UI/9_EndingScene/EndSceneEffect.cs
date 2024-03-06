using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndSceneEffect : MonoBehaviour
{
    [SerializeField]
    GameObject _backGround;

    [SerializeField]
    GameObject _textBox;

    [SerializeField]
    GameObject _mainText;

    [SerializeField]
    GameObject _button;

    private void Start()
    {
        StartCoroutine(Flow());
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

        // 버튼 open
        _button.SetActive(true);
        _button.GetComponent<Button>().Select();
    }


    public void ExitBt()
    {
        // 플레이어 프리팹 리셋 후 리플레이 true, 메인 씬으로 이동
        PlayerBulletPoolManager.Instance.ResetPrefab();
        GameManager.Instance.SetIsReplay(true);
        SceneManager.LoadScene("MainScene");
    }
}
