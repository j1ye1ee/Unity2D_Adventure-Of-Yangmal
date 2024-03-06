using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class StageFailBt : BaseButton
{
    // �÷��̾� ����� �ߴ� ui
    // ok --> stage1 ��ŸƮ������ �̵�
    // no --> ���ø����̼� ���� & �÷��̾� �Ѿ� ������ ������ �ʱ�ȭ

    public GameObject _fadeOut;
    public string _nextScene;


    void Start()
    {
        _firstSelect.Select();
    }

    // ok
    public override void OkButton()
    {
        StartCoroutine(EnterToNextScene(_nextScene));
    }

    // startRoom ���� ���̵� �ڷ�ƾ
    IEnumerator EnterToNextScene(string name)
    {
        // ���ӿ���ó��
        GameManager.Instance.SetIsGameOver(true);

        // ���̵� �ƿ� ȿ��
        _fadeOut.SetActive(true);
        StartCoroutine(_fadeOut.GetComponent<UiEffect>().FadeOut());
        yield return new WaitUntil(
            () => _fadeOut.GetComponent<UiEffect>()._isFadeOutEnd == true);

        // ���̵� �ƿ� �Ϸ� �� ui �ʱ�ȭ
        for (int i = 0; i < gameObject.transform.parent.childCount; i++)
        {
            gameObject.transform.parent.GetChild(i).gameObject.SetActive(false);
        }

        // �ε��
        SceneManager.LoadScene(name);
    }


    // ��������
    public override void CloseUI()
    {
        // �÷��̾� ������ ���� �� ���� ����
        PlayerBulletPoolManager.Instance.ResetPrefab();
        Application.Quit();
    }


}
