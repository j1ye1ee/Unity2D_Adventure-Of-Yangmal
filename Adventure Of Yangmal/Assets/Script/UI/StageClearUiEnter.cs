using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageClearUiEnter : UiEffect
{
    // ���͸� ������ ������ ���� �� �ε�
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
        _isStartCoroutine = true;
        StartCoroutine(FadeOut());
        yield return new WaitUntil(() => _isFadeOutEnd == true);

        // ���̵� �ƿ� �Ϸ� �� �ε��
        SceneManager.LoadScene(name);
    }
}
