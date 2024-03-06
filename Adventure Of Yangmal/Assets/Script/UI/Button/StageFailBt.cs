using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class StageFailBt : BaseButton
{
    // 플레이어 사망시 뜨는 ui
    // ok --> stage1 스타트룸으로 이동
    // no --> 어플리케이션 종료 & 플레이어 총알 프리팹 스케일 초기화

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

    // startRoom 으로 씬이동 코루틴
    IEnumerator EnterToNextScene(string name)
    {
        // 게임오버처리
        GameManager.Instance.SetIsGameOver(true);

        // 페이드 아웃 효과
        _fadeOut.SetActive(true);
        StartCoroutine(_fadeOut.GetComponent<UiEffect>().FadeOut());
        yield return new WaitUntil(
            () => _fadeOut.GetComponent<UiEffect>()._isFadeOutEnd == true);

        // 페이드 아웃 완료 후 ui 초기화
        for (int i = 0; i < gameObject.transform.parent.childCount; i++)
        {
            gameObject.transform.parent.GetChild(i).gameObject.SetActive(false);
        }

        // 로드씬
        SceneManager.LoadScene(name);
    }


    // 게임종료
    public override void CloseUI()
    {
        // 플레이어 프리팹 리셋 후 게임 종료
        PlayerBulletPoolManager.Instance.ResetPrefab();
        Application.Quit();
    }


}
