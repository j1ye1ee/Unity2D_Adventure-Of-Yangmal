using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageClearUiEnter : UiEffect
{
    // 엔터를 누르면 지정된 다음 씬 로드
    public string _nextSceneName;
    bool _isStartCoroutine = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            if (!_isStartCoroutine)
            {
                StartCoroutine(EnterToNextScene(_nextSceneName));
            }
    }

    IEnumerator EnterToNextScene(string name)
    {
        Time.timeScale = 1;
        // 남은 공격에 플레이어가 당하지 않도록 처리 취소
        PlayerStatus.Instance.gameObject.GetComponent<Collider2D>().enabled = true;

        _isStartCoroutine = true;
        StartCoroutine(FadeOut());
        yield return new WaitUntil(() => _isFadeOutEnd == true);

        // 페이드 아웃 완료 후 로드씬
        SceneManager.LoadScene(name);
    }
}
