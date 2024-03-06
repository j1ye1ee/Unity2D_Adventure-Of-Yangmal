using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossRoomEnterBt : BaseButton
{
    // ok --> ���� ������ �̵�
    // no --> â�ݱ� * �÷��̾� �ӵ� �ǵ�������

    public GameObject _fadeOut;
    public string _nextScene;
    public GameObject _door;


    void Start()
    {
        _firstSelect.Select();
    }

    // ok
    public override void OkButton()
    {
        StartCoroutine(EnterToNextScene(_nextScene));
    }


    IEnumerator EnterToNextScene(string name)
    {
        _fadeOut.SetActive(true);
        StartCoroutine(_fadeOut.GetComponent<UiEffect>().FadeOut());
        yield return new WaitUntil(
            () => _fadeOut.GetComponent<UiEffect>()._isFadeOutEnd == true);
        // ���̵� �ƿ� �Ϸ� �� �÷��̾� ���󺹱� & �ε��
        UiManager.Instance.ReturnPlayer(PlayerStatus.Instance.gameObject);
        SceneManager.LoadScene(name);
    }

    public override void CloseUI(GameObject shut)
    {
        Debug.Log("â�ݱ�");

        // ���� �� �Ա� �ݶ��̴� ����
        _door.gameObject.GetComponent<Boss1EnterDoor>().StartColliderCoroutine();
        // �÷��̾� ����
        UiManager.Instance.ReturnPlayer(PlayerStatus.Instance.gameObject);
        // â�ݱ�
        shut.SetActive(false);
    }



}
