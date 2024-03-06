using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSetting : MonoBehaviour
{
    [SerializeField]
     protected string _stageName;

    // �÷��̾� ��ġ ����
    public void PlayerTrsfReset()
    {
        PlayerStatus.Instance.gameObject.transform.position = new Vector3(0, 0, 0);
    }

    // ui ����
    public void SetStartUi()
    {
        UiManager.Instance.SetStageText(_stageName);
        UiManager.Instance.SetPlayerDamage();
        UiManager.Instance.SetPlayerDex();
        UiManager.Instance.SetPlayerGold();
        GameObject.FindWithTag("HpManager").GetComponent<HpManager>().HeartSet();
        
    }

    // ���ʿ��� dontDestroyOnload setOff
    public void SetOff(GameObject obj)
    {
        obj.gameObject.SetActive(false);
    }

    // dontDestroyOnLoad setOn
    public void SetOn(GameObject obj)
    {
        obj.gameObject.SetActive(true);
    }
}
