using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossRoomEnterBt : BaseButton
{
    // ok --> 보스 방으로 이동
    // no --> 창닫기 * 플레이어 속도 되돌려놓기

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
        // 페이드 아웃 완료 후 플레이어 원상복구 & 로드씬
        UiManager.Instance.ReturnPlayer(PlayerStatus.Instance.gameObject);
        SceneManager.LoadScene(name);
    }

    public override void CloseUI(GameObject shut)
    {
        Debug.Log("창닫기");

        // 보스 방 입구 콜라이더 원복
        _door.gameObject.GetComponent<Boss1EnterDoor>().StartColliderCoroutine();
        // 플레이어 원복
        UiManager.Instance.ReturnPlayer(PlayerStatus.Instance.gameObject);
        // 창닫기
        shut.SetActive(false);
    }



}
