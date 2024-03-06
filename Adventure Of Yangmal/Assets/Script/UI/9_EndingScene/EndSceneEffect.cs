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
        // ��� ���̵���

        _backGround.SetActive(true);
        StartCoroutine(_backGround.GetComponent<UiEffect>().FadeIn());

        yield return new WaitUntil(() => _backGround.GetComponent<UiEffect>()._isFadeInEnd == true);
        _backGround.GetComponent<UiEffect>()._isFadeInEnd = false;

        // �ؽ�Ʈ���� open
        _textBox.SetActive(true);
        StartCoroutine(_textBox.GetComponent<UiEffect>().Spread());

        yield return new WaitUntil(() => _textBox.GetComponent<UiEffect>()._isSpreadEnd == true);
        _textBox.GetComponent<UiEffect>()._isSpreadEnd = false;


        // �ؽ�Ʈ open
        _mainText.SetActive(true);

        // ��ư open
        _button.SetActive(true);
        _button.GetComponent<Button>().Select();
    }


    public void ExitBt()
    {
        // �÷��̾� ������ ���� �� ���÷��� true, ���� ������ �̵�
        PlayerBulletPoolManager.Instance.ResetPrefab();
        GameManager.Instance.SetIsReplay(true);
        SceneManager.LoadScene("MainScene");
    }
}
