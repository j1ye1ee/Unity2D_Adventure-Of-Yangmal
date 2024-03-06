using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ReadySceneSet : SceneSetting
{
    public GameObject _ui;
    public string _nextSceneName;



    void Awake()
    {
        // �÷��̾� ��ġ ���� �� setOff
        PlayerTrsfReset();
        SetOff(PlayerStatus.Instance.gameObject);

        // Main ui  setoff
        SetOff(UiManager.Instance.transform.parent.gameObject);

        StartCoroutine(_ui.GetComponent<EndStageUi>().Flow());
        Invoke("GotoNextScene", 5f);

    }


    void GotoNextScene()
    {
        SceneManager.LoadScene(_nextSceneName);
    }
}
